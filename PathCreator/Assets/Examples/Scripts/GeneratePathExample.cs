using UnityEngine;

namespace PathCreation.Examples
{
    // Example of creating a path at runtime from a set of points.
    // Moves along the generated path.

    [RequireComponent(typeof(TrailRenderer))]
    public class GeneratePathExample : MonoBehaviour
    {
        public Transform[] waypoints;
        public float speed = 8;

        float dstTravelled;
        VertexPath path;

        void Start()
        {
            if (waypoints.Length > 0)
            {
                // Create a new bezier path from the waypoints.
                // The 'true' argument specifies that the path should be a closed loop
                BezierPath bezierPath = new BezierPath(waypoints, true, PathSpace.xyz);
                // Create a vertex path from the bezier path
                path = new VertexPath(bezierPath);
            }
        }

        void Update()
        {
            if (path != null)
            {
                dstTravelled += speed * Time.deltaTime;
                transform.position = path.GetPointAtDistance(dstTravelled);
            }
        }

    }
}