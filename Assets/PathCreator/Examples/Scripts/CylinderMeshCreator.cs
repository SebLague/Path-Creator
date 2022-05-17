using System.Collections.Generic;
using PathCreation.Utility;
using UnityEngine;

namespace PathCreation.Examples {
	public class CylinderMeshCreator : PathSceneTool {

		public float thickness = .15f;
		[Range (3, 30)]
		public int resolutionU = 10;
		[Min (0)]
		public float resolutionV = 20;

		public Material material;

		[SerializeField, HideInInspector]
		GameObject meshHolder;

		MeshFilter meshFilter;
		MeshRenderer meshRenderer;
		Mesh mesh;

		protected override void PathUpdated () {
			if (pathCreator != null) {
				AssignMeshComponents ();
				AssignMaterials ();
				CreateMesh ();
			}
		}

		void CreateMesh () {
			List<Vector3> verts = new List<Vector3> ();
			List<int> triangles = new List<int> ();

			int numCircles = Mathf.Max (2, Mathf.RoundToInt (path.length * resolutionV) + 1);
			var pathInstruction = PathCreation.EndOfPathInstruction.Stop;

			for (int s = 0; s < numCircles; s++) {
				float segmentPercent = s / (numCircles - 1f);
				Vector3 centerPos = path.GetPointAtTime (segmentPercent, pathInstruction);
				Vector3 norm = path.GetNormal (segmentPercent, pathInstruction);
				Vector3 forward = path.GetDirection (segmentPercent, pathInstruction);
				Vector3 tangentOrWhatEver = Vector3.Cross (norm, forward);

				for (int currentRes = 0; currentRes < resolutionU; currentRes++) {
					var angle = ((float) currentRes / resolutionU) * (Mathf.PI * 2.0f);

					var xVal = Mathf.Sin (angle) * thickness;
					var yVal = Mathf.Cos (angle) * thickness;

					var point = (norm * xVal) + (tangentOrWhatEver * yVal) + centerPos;
					verts.Add (point);

					//! Adding the triangles
					if (s < numCircles - 1) {
						int startIndex = resolutionU * s;
						triangles.Add (startIndex + currentRes);
						triangles.Add (startIndex + (currentRes + 1) % resolutionU);
						triangles.Add (startIndex + currentRes + resolutionU);

						triangles.Add (startIndex + (currentRes + 1) % resolutionU);
						triangles.Add (startIndex + (currentRes + 1) % resolutionU + resolutionU);
						triangles.Add (startIndex + currentRes + resolutionU);
					}

				}
			}

			if (mesh == null) {
				mesh = new Mesh ();
			} else {
				mesh.Clear ();
			}

			mesh.SetVertices (verts);
			mesh.SetTriangles (triangles, 0);
			mesh.RecalculateNormals ();

		}

		// Add MeshRenderer and MeshFilter components to this gameobject if not already attached
		void AssignMeshComponents () {

			if (meshHolder == null) {
				meshHolder = new GameObject ("Mesh Holder");
			}

			meshHolder.transform.rotation = Quaternion.identity;
			meshHolder.transform.position = Vector3.zero;
			meshHolder.transform.localScale = Vector3.one;

			// Ensure mesh renderer and filter components are assigned
			if (!meshHolder.gameObject.GetComponent<MeshFilter> ()) {
				meshHolder.gameObject.AddComponent<MeshFilter> ();
			}
			if (!meshHolder.GetComponent<MeshRenderer> ()) {
				meshHolder.gameObject.AddComponent<MeshRenderer> ();
			}

			meshRenderer = meshHolder.GetComponent<MeshRenderer> ();
			meshFilter = meshHolder.GetComponent<MeshFilter> ();
			if (mesh == null) {
				mesh = new Mesh ();
			}
			meshFilter.sharedMesh = mesh;
		}

		void AssignMaterials () {
			if (material != null) {
				meshRenderer.sharedMaterial = material;
			}
		}

	}
}