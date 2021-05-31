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
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace PathCreationEditor
{
  public class PathDataContainer
  {
    private static Dictionary<string, PathData> data = new Dictionary<string, PathData>();

    public static Dictionary<string, PathData> GetData()
    {
      return PathDataContainer.data;
    }

    public static void SaveData(PathData data)
    {
      PathDataContainer.data[data.name] = data;
    }

    public static IEnumerable<Vector3> GetPoints(PathData pathData)
    {
      var points = pathData.points;
      var index = 0;

      foreach(var point in points)
      {
        var isFirstPoint = index == 0;
        var isLastPoint  = index == points.Count() - 1;

        if(!isFirstPoint)
        {
          yield return point.handles[0].GetVector3();
        }

        yield return point.GetVector3();

        if(!isLastPoint)
        {
          yield return point.handles[1].GetVector3();
        }

        index++;
      }
    }

    public static IEnumerable<float> GetNormals(PathData pathData)
    {
      foreach(var point in pathData.points)
      {
        yield return -point.tilt * Mathf.Rad2Deg;
      }
    }
  }
}
