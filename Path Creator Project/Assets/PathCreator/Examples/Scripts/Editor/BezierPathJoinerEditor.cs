using UnityEngine;
using UnityEditor;

namespace PathCreation.Examples {

	[CustomEditor(typeof(BezierPathJoiner))]
	public class BezierPathJoinerEditor : Editor {

		public override void OnInspectorGUI() {
			DrawDefaultInspector();

			BezierPathJoiner pathJoiner = (BezierPathJoiner)target;
			if (GUILayout.Button("Join Paths")) {
				pathJoiner.JoinPaths();
			}
		}
	}
}