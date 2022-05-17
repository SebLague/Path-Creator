using UnityEngine;
using System.Collections.Generic;

namespace PathCreation.Examples {

	/// <summary>
	/// This component stiches multiple BezierPaths smoothly together into a single one. 
	/// multiple BezierPaths that are part of PathCreators together and creates a single resulting path, 
	/// which then gets applied to the PathCreator component on this gameobject. Useful for stiching togteher multiple paths into one.
	/// </summary>
	[RequireComponent(typeof(PathCreator))]
	public class BezierPathJoiner : MonoBehaviour {

		[SerializeField]
		List<PathCreator> pathCreatorsToJoin = new List<PathCreator>();
		PathCreator pathCreator = null;

		/// <summary> Stiches the BezierPaths of the referenced PathCreators together. </summary>
		public void JoinPaths() {
			JoinPaths(pathCreatorsToJoin);
		}

		/// <summary> Stiches the BezierPaths of multiple PathCreators smoothly together and applies the path to the PathCreator on this GameObject.</summary>
		/// <param name="pathCreatorList">A list of PathCreators whose BezierPaths will be joined to a single path.</param>
		void JoinPaths(List<PathCreator> pathCreatorList) {
			if (pathCreator == null)
				pathCreator = GetComponent<PathCreator>();

			if (pathCreatorList.Count < 2) {
				Debug.LogError("There need to be at least two PathCreators referenced to join them together!");
				return;
            }

			// copy the first path - it will be our starting path
			PathCreator startingCreator = pathCreatorList[0];
			BezierPath joinedPath = new BezierPath(pathCreatorList[0].bezierPath);

			// iterate over all subsequent paths to add their points to our first path
			for(int i = 1; i < pathCreatorList.Count; i++) {
				PathCreator creator = pathCreatorList[i];
				if (creator == null)
					continue;

				BezierPath path = creator.bezierPath;
				if (path == null || path.NumPoints == 0)
					continue;

				// create point list to append to our joinedPath
				List<Vector3> points = new List<Vector3>();

				// Calculate points halfway between paths
				// -- This is needed because bezier paths are missing tangent points at before their startpoint and after their endpoint.
				// -- When just appending the points of the next path, the first actual path waypoint would be treated as the tangent point of the last paths endpoint.
				// -- Thus, we need to create two tangent points in betweeen, one for the last point of the last path, and one for the first point of the next path.
				PathCreator lastCreator = pathCreatorList[i - 1];
				Vector3 lastPoint = lastCreator.transform.rotation * lastCreator.bezierPath[lastCreator.bezierPath.NumPoints - 1];
				Vector3 lastTangent = lastCreator.transform.rotation * lastCreator.bezierPath[lastCreator.bezierPath.NumPoints - 2];
				Vector3 firstPoint = creator.transform.rotation * path[0];
				Vector3 firstTangent = creator.transform.rotation * path[1];

				// the new tangent points that would otherwise be missing between both paths
				Vector3 lastPointNewTangent = lastPoint + (lastPoint - lastTangent);
				Vector3 firstPointNewTangent = firstPoint + (firstPoint - firstTangent);

				// adjust for worldposition of creators
				lastPointNewTangent += lastCreator.transform.position - startingCreator.transform.position;
				firstPointNewTangent += creator.transform.position - startingCreator.transform.position;

				// add the two missing tangent points in between both paths
				points.Add(lastPointNewTangent);
				points.Add(firstPointNewTangent);

				// add the rest of the points to our path
                for (int point = 0; point < path.NumPoints; point++) {
					// add point to list and compensate for different transform positions and rotations of creators
					points.Add((creator.transform.rotation * path[point]) + creator.transform.position - startingCreator.transform.position);
                }
                joinedPath.AddPoints(points);
            }

			// apply the created path to our PathCreator
			pathCreator.bezierPath = joinedPath;
		}
    }
}