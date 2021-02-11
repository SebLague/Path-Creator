///
///
/// @copyright (c) by MEMPIC LTD
/// @copyright (c) by WWW.MEMPIC.COM
///
///
/// @license http://www.mempic.com/licenses/private
///
/// By exercising the licensed rights you accept and agree to be bound by the
/// terms and conditions of this @license. To the extent this @license
/// may be interpreted as a contract, you are granted the licensed rights
/// in consideration of your acceptance of these terms and conditions,
/// and the licensor grants you such rights in consideration of benefits
/// the licensor receives from making the licensed material available
/// under these terms and conditions.
///
///
using UnityEngine;

namespace PathCreation
{
  public partial class BezierPath
  {
    protected bool isCleared;

    public BezierPath(BezierPath target)
    {
      this.points = target.points;
    }

    public void ClearPath()
    {
      int minPointsCount = 4;

      int attempt = 0;
      while(points.Count > minPointsCount && ++attempt < 10000)
      {
        DeleteSegment(0);
      }

      for(int i = 0; i < NumPoints; i++)
      {
        SetPoint(i, Vector3.forward * i);
      }

      isCleared = true;
    }

    public void EncapsulatePath(PathCreator pathCreator)
    {
      if(!isCleared)
      {
        Vector3 lastAnchorSecondControl = points[points.Count - 1];
        Vector3 firstAnchorSecondControl = points[points.Count - 1];

        points.Add(lastAnchorSecondControl);
        points.Add(firstAnchorSecondControl);
      }

      var bezierPath = pathCreator.bezierPath;
      for(int i = 0; i < bezierPath.NumPoints; i++)
      {
        if(!isCleared)
        {
          points.Add(pathCreator.transform.TransformPoint(bezierPath.GetPoint(i)));

          if(i == 0)
          {
            var anchorsDifferenceForward = points[points.Count - 1] - points[points.Count - 4];
            var anchorsDifferenceBackward = -anchorsDifferenceForward;

            SetPoint(points.Count - 3, points[points.Count - 4] + anchorsDifferenceForward * 0.2f);
            SetPoint(points.Count - 2, points[points.Count - 1] + anchorsDifferenceBackward * 0.2f);
          }
        }
        else
        {
          SetPoint(i, pathCreator.transform.TransformPoint(bezierPath.GetPoint(i)));
        }

        if(isCleared && i == 3)
        {
          isCleared = false;
        }
      }

      perAnchorNormalsAngle.Clear();
      for(int i = 0; i < bezierPath.NumAnchorPoints; i++)
      {
        perAnchorNormalsAngle.Add(bezierPath.perAnchorNormalsAngle[i]);
      }

      NotifyPathModified();
    }
  }
}
