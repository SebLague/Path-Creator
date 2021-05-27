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
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

namespace PathCreationEditor
{
  public class PathPostprocessor : AssetPostprocessor
  {
    private static List<PathImporter> importers = new List<PathImporter>() {
      new PathDataImporter(),
      new PathPrefabsImporter(),
    };

    protected static void OnPostprocessAllAssets(
      string[] importedAssets,
      string[] deletedAssets,
      string[] movedAssets,
      string[] movedFromAssetPaths
    )
    {
      foreach(var importer in importers)
      {
        foreach(string path in importedAssets)
        {
          importer.Import(path);
        }
      }

      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
    }
  }
}
