using System;
using UnityEngine;
using PathCreation;
using PathCreation.Utility;

namespace PathCreation.Examples
{
	public class GetClosestPointOnPathTest : MonoBehaviour
	{
		public PathCreator pathCreator;

        public Transform inputPositionObject;
        public Transform oldMethodObject;
        public Transform newMethodObject;

		void Update()
		{
            // Get a point around the path
            Vector3 worldPoint = pathCreator.path.GetPointAtDistance(Time.time * 2) + new Vector3(Mathf.Sin(Time.time)*2, Mathf.Sin(1+Time.time*0.5f)*2.5f, Mathf.Sin(2+Time.time*0.3f)*3);

            UnityEngine.Profiling.Profiler.BeginSample("Before Optimization");
            Vector3 oldClosestPointOnPath = OldGetClosestPointOnPath(pathCreator.path, worldPoint);
            float oldClosestTimeOnPath = OldGetClosestTimeOnPath(pathCreator.path, worldPoint);
            float oldClosestDistanceAlongPath = OldGetClosestDistanceAlongPath(pathCreator.path, worldPoint);
            UnityEngine.Profiling.Profiler.EndSample();

            UnityEngine.Profiling.Profiler.BeginSample("After Optimization");
            Vector3 newClosestPointOnPath = pathCreator.path.GetClosestPointOnPath(worldPoint);
            float newClosestTimeOnPath = pathCreator.path.GetClosestTimeOnPath(worldPoint);
            float newClosestDistanceAlongPath = pathCreator.path.GetClosestDistanceAlongPath(worldPoint);
            UnityEngine.Profiling.Profiler.EndSample();

            // This much error we allow between the new and old implementations
            float errorThreshold = 0.001f;

            // Check if the GetClosestPointOnPath returned value is the same for the old and new implementation
            if (Vector3.Distance(newClosestPointOnPath, oldClosestPointOnPath) > errorThreshold)
                Debug.LogErrorFormat(this, "GetClosestPointOnPath() failed (distance={0})", Vector3.Distance(newClosestPointOnPath, oldClosestPointOnPath));

            // Check if the GetClosestTimeOnPath returned value is the same for the old and new implementation
            if (Mathf.Abs(newClosestTimeOnPath - oldClosestTimeOnPath) > errorThreshold)
                Debug.LogErrorFormat(this, "GetClosestTimeOnPath() failed. new={0}, old={1}", newClosestTimeOnPath, oldClosestTimeOnPath);

            // Check if the GetClosestDistanceAlongPath returned value is the same for the old and new implementation
            if (Mathf.Abs(newClosestDistanceAlongPath - oldClosestDistanceAlongPath) > errorThreshold)
                Debug.LogErrorFormat(this, "GetClosestDistanceAlongPath() failed. new={0}, old={1}", newClosestDistanceAlongPath, oldClosestDistanceAlongPath);

            // Visualize the input position
            inputPositionObject.position = worldPoint;

            // Visualize the computed path position
            oldMethodObject.position = oldClosestPointOnPath;
            newMethodObject.position = newClosestPointOnPath;

            // Draw lines to connect the input position and the calculated output positions
            Debug.DrawLine(worldPoint, oldClosestPointOnPath, Color.red);
            Debug.DrawLine(worldPoint, newClosestPointOnPath, Color.green);
        }

        // This is the old VertexPath code before the optimization. I copied it here to compare its
        // results to the new implementation, to make sure it still returns the same values after the optimization.
        Vector3 OldGetClosestPointOnPath(VertexPath path, Vector3 worldPoint)
        {
            VertexPath.TimeOnPathData data = OldCalculateClosestPointOnPathData(path, worldPoint);
            return Vector3.Lerp(path.GetPoint(data.previousIndex), path.GetPoint(data.nextIndex), data.percentBetweenIndices);
        }

        float OldGetClosestTimeOnPath(VertexPath path, Vector3 worldPoint)
        {
            VertexPath.TimeOnPathData data = OldCalculateClosestPointOnPathData(path, worldPoint);
            return Mathf.Lerp(path.times[data.previousIndex], path.times[data.nextIndex], data.percentBetweenIndices);
        }

        float OldGetClosestDistanceAlongPath(VertexPath path, Vector3 worldPoint)
        {
            VertexPath.TimeOnPathData data = OldCalculateClosestPointOnPathData(path, worldPoint);
            return Mathf.Lerp(path.cumulativeLengthAtEachVertex[data.previousIndex], path.cumulativeLengthAtEachVertex[data.nextIndex], data.percentBetweenIndices);
        }

        VertexPath.TimeOnPathData OldCalculateClosestPointOnPathData(VertexPath path, Vector3 worldPoint)
        {
            Vector3[] localPoints = path.localNormals;
            bool isClosedLoop = path.isClosedLoop;

            float minSqrDst = float.MaxValue;
            Vector3 closestPoint = Vector3.zero;
            int closestSegmentIndexA = 0;
            int closestSegmentIndexB = 0;

            for (int i = 0; i < localPoints.Length; i++)
            {
                int nextI = i + 1;
                if (nextI >= localPoints.Length)
                {
                    if (isClosedLoop)
                    {
                        nextI %= localPoints.Length;
                    }
                    else
                    {
                        break;
                    }
                }

                Vector3 closestPointOnSegment = MathUtility.ClosestPointOnLineSegment(worldPoint, path.GetPoint(i), path.GetPoint(nextI));
                float sqrDst = (worldPoint - closestPointOnSegment).sqrMagnitude;
                if (sqrDst < minSqrDst)
                {
                    minSqrDst = sqrDst;
                    closestPoint = closestPointOnSegment;
                    closestSegmentIndexA = i;
                    closestSegmentIndexB = nextI;
                }

            }
            float closestSegmentLength = (path.GetPoint(closestSegmentIndexA) - path.GetPoint(closestSegmentIndexB)).magnitude;
            float t = (closestPoint - path.GetPoint(closestSegmentIndexA)).magnitude / closestSegmentLength;
            return new VertexPath.TimeOnPathData(closestSegmentIndexA, closestSegmentIndexB, t);
        }
    }
}
