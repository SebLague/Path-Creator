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

      perAnchorNormalsAngle.Clear();
      for(int i = 0; i < NumAnchorPoints; i++)
      {
        perAnchorNormalsAngle.Add(0);
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
      else
      {
        perAnchorNormalsAngle.Clear();
      }

      var bezierPath = pathCreator.bezierPath;
      for(int i = 0; i < bezierPath.NumPoints; i++)
      {
        if(!isCleared)
        {
          points.Add(pathCreator.transform.TransformPoint(bezierPath.GetPoint(i)));

          if(i == 1)
          {
            //soft direction after merge based on previous control points
            var endConnectionPoint = points[points.Count - 5] - points[points.Count - 6];
            var startConnectionPoint = points[points.Count - 2] - points[points.Count - 1];
            var anchorsDistance = points[points.Count - 2] - points[points.Count - 5];

            SetPoint(points.Count - 4, points[points.Count - 5] + endConnectionPoint.normalized * anchorsDistance.magnitude * 0.4f);
            SetPoint(points.Count - 3, points[points.Count - 2] + startConnectionPoint.normalized * anchorsDistance.magnitude * 0.4f);
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

        if(i % 3 == 0)
        {
          float normalsDifference = this.globalNormalsAngle - bezierPath.globalNormalsAngle;
          perAnchorNormalsAngle.Add(bezierPath.perAnchorNormalsAngle[i / 3] + normalsDifference);
        }
      }

      NotifyPathModified();
    }
  }
}
