using UnityEngine;

namespace PathCreation.Examples
{
	public class GetClosestPointOnPathTest : MonoBehaviour
	{
		public PathCreator pathCreator;
		public Transform objectOnPath;

		void Update()
		{
			// Wait for the PathFollower Component to set objectOnPath on the path
			if (Time.timeSinceLevelLoad < 0.1f)
				return;

			// Since objectOnPath travels on the path, the actualWorldPoint and closestPointOnPath
			// should be pretty close to each other.
			Vector3 actualWorldPoint = objectOnPath.position;
			Vector3 closestPointOnPath = pathCreator.path.GetClosestPointOnPath(actualWorldPoint);

			var distance = Vector3.Distance(actualWorldPoint, closestPointOnPath);
			if (distance > 0.001f)
				Debug.LogErrorFormat(this, "Points too far apart (distance={0})", distance);
		}
	}
}
