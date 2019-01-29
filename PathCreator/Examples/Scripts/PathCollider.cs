using UnityEngine;
using PathCreation;

[RequireComponent(typeof(PathCreator))]
public class PathCollider : MonoBehaviour
{
    public PathCreator pathCreator;
    public EdgeCollider2D edge;

    void Start()
    {
        pathCreator = GetComponent<PathCreator>();
        if (pathCreator != null)
            {
            edge = gameObject.AddComponent<EdgeCollider2D>();
            Vector3[] verts = pathCreator.path.vertices;
            Vector2[] verts2D = new Vector2[verts.Length];
            for (int i = 0; i < verts.Length; i++)
                {
                verts2D[i] = new Vector2(verts[i].x, verts[i].y);
                }
            edge.points = verts2D;
            }
    }
}
