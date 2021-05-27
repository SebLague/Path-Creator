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
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

using YamlDotNet.Core;

namespace PathCreationEditor
{
  public class PathDataImporter : PathImporter
  {
    protected const string serializedDataFileExtension = ".json";

    public PathDataImporter()
    {
      var assets = AssetDatabase.FindAssets("t:TextAsset");

      foreach(var asset in assets)
      {
        Import(AssetDatabase.GUIDToAssetPath(asset));
      }
    }

    public override void Import(string path)
    {
      if(path.Contains(serializedDataFileExtension))
      {
        try
        {
          var serializedData = AssetDatabase.LoadAssetAtPath<TextAsset>(path).text;
          var deserializedData = Mempic.Yaml.Deserialize<PathData[]>(serializedData);

          foreach(var data in deserializedData)
          {
            PathDataContainer.SaveData(data);
          }
        }
        catch(YamlException)
        {
        }
      }
    }
  }
}
