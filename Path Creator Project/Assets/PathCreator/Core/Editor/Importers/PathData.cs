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
using System;

using UnityEngine;
using UnityEditor;

namespace PathCreationEditor
{
  [Serializable]
  public class PathData
  {
    public string name;
    public Point[] points;

    [Serializable]
    public class Handle
    {
      public float x;
      public float y;
      public float z;

      public Vector3 GetVector3()
      {
        return new Vector3(-x, z, -y);
      }
    }

    [Serializable]
    public class Point
    {
      public float x;
      public float y;
      public float z;
      public float tilt;
      public Handle[] handles;

      public Vector3 GetVector3()
      {
        return new Vector3(-x, z, -y);
      }
    }
  }
}
