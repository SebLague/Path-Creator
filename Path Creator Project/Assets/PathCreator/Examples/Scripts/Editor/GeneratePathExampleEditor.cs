using UnityEngine;
using UnityEditor;
using PathCreation;

namespace PathCreation.Examples
{
    [CustomEditor(typeof(GeneratePathExample), true)]
    public class GeneratePathExampleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GeneratePathExample generatePathExample = (GeneratePathExample) target;
            if (GUILayout.Button("Generate Path"))
            {
                generatePathExample.GeneratePath();
            }
        }
    }   
}