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
using Mempic;

using System;
using System.Linq;

using UnityEngine;
using UnityEditor;

namespace PathCreationEditor
{
  public class PathPrefabsImporter : PathImporter
  {
    public override void Import(string path)
    {
      var root = AssetDatabase.LoadAssetAtPath<GameObject>(path);

      if(root)
      {
        var pathCreator = root.GetComponentInChildren<PathCreation.PathCreator>();

        if(pathCreator)
        {
          var meshFilter = root.GetComponent<MeshFilter>();

          if(meshFilter)
          {
            var mesh = meshFilter.sharedMesh;

            if(mesh)
            {
              var key = mesh.name;

              if(PathDataContainer.GetData().ContainsKey(key))
              {
                var data = PathDataContainer.GetData()[key];

                pathCreator.bezierPath = new PathCreation.BezierPath(
                  PathDataContainer.GetPoints(data),
                  pathCreator.bezierPath.IsClosed,
                  pathCreator.bezierPath.Space,
                  pathCreator.bezierPath.ControlPointMode,
                  PathDataContainer.GetNormals(data).ToList()
                );

                pathCreator.bezierPath.FlipNormals = false;
                pathCreator.bezierPath.NotifyPathModified();

                EditorUtility.SetDirty(root);

                Logs.EditorLog(
                  string.Join(
                    Environment.NewLine,
                    "Path data for candidate has been successfully imported",
                    key
                  )
                );
              }
              else
              {
                Logs.EditorLogWarning(
                  string.Join(
                    Environment.NewLine,
                    "There is no path data found for candidate",
                    key
                  )
                );
              }
            }
            else
            {
              Logs.EditorLogError(
                string.Join(
                  Environment.NewLine,
                  "Trying to apply path data but there are missing mesh",
                  root
                )
              );
            }
          }
        }
      }
    }
  }
}
