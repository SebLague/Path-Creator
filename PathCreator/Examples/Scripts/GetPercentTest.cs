using UnityEngine;
using PathCreation;

public class GetPercentTest : MonoBehaviour
{
    public PathCreator p;
    public GameObject pathWalker;

    private void Start()
        {
        if (p == null)
            {
            p = FindObjectOfType<PathCreator>();
            }
        }

    private void Update()
        {
        if (p != null)
            {
            float t = p.path.CalculatePercentByPosition(transform.position);
            Vector3 pos = p.path.GetPoint(t);
            Debug.Log(t);
            pathWalker.transform.position = pos;
            }
        }
    }
