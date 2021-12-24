using UnityEngine;

namespace PathCreation.Examples {
    // Example of creating a path from a set of points upon button press in the editor.
    [RequireComponent(typeof(PathCreator))]
    public class GeneratePathExample : MonoBehaviour
    {
        public bool closedLoop = true;
        public Transform[] waypoints = new Transform[0];

        public void GeneratePath()
        {
            if (waypoints.Length > 0)
            {
                // Create a new bezier path from the waypoints.
                BezierPath bezierPath = new BezierPath(waypoints, closedLoop, PathSpace.xyz);
                GetComponent<PathCreator>().bezierPath = bezierPath;
            }
        }
    }
}