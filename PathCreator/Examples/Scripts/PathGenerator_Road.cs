using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using UnityEngine.U2D;


/// <summary>
/// Path generator that generates a path procedurally and creates a road mesh that matches it.
/// </summary>

[RequireComponent(typeof(PathCreator))]
[RequireComponent(typeof(PathCollider))]
[RequireComponent(typeof(RoadMeshCreator))]
public class PathGenerator_Road : MonoBehaviour
    {
    public float minDistance = 5f, maxDistance = 20f;
    public float minHeight = 2f, maxHeight = 10f;

    [Range(0.5f, 5f)]
    public float timeBetweenSpawns = 0.5f;
    [Range(0, 20)]
    public int pointsBeforeRebuild = 10;
    public PathCreator pathCreator;
    private RoadMeshCreator roadMesh;
    private PathCollider pathCollider;


    public PhysicsMaterial2D physicsMaterial2D;
    public Material material;

    private Vector3 lastVert;

    public List<Vector2> pathPoints;
    private Vector2 lastPoint;
    [SerializeField]
    private bool spawn = true;
    private int sign = 1;

    void Start()
        {
        SetRoadMesh();
        SetPathCreator();
        StartCoroutine(IGeneratePath());
        }
    private void Update()
        {
        if (Input.GetMouseButtonDown(2))
            {
            spawn = !spawn;
            if (spawn)
                StartCoroutine(IGeneratePath());
            }
        }
    private IEnumerator IGeneratePath()
        {
        WaitForSeconds wait = new WaitForSeconds(timeBetweenSpawns);
        while (spawn)
            {
            for (int i = 0; i < pointsBeforeRebuild; i++)
                {
                GenerateRandomPoint();
                }
            yield return wait;
            RebuildPath();
            }
        }

    private void GenerateRandomPoint()
        {
        float x = Random.Range(minDistance, maxDistance);
        float y = Random.Range(minHeight, maxHeight) * sign;
        sign *= -1;
        Vector2 newPoint = new Vector2(x, y) + lastPoint;
        pathPoints.Add(newPoint);
        lastPoint = newPoint;
        }

    private void RebuildPath()
        {
        pathCreator.bezierPath = new BezierPath(pathPoints, false, PathSpace.xy);
        pathCreator.bezierPath.ControlPointMode = BezierPath.ControlMode.Mirrored;
        SetPathCollider();
        }

    private void SetPathCreator()
        {
        pathCreator = GetComponent<PathCreator>();
        pathPoints = new List<Vector2>();

        pathPoints.Add(Vector2.zero);
        pathPoints.Add(Vector2.right * 5 - Vector2.up);

        for (int i = 0; i < 10; i++)
            {
            GenerateRandomPoint();
            }

        lastPoint = pathPoints[pathPoints.Count - 1];
        pathCreator.bezierPath.ControlPointMode = BezierPath.ControlMode.Mirrored;
        pathCreator.bezierPath = new BezierPath(pathPoints, false, PathSpace.xy);
        SetPathCollider();
        }

    private void SetPathCollider()
        {
        pathCollider = GetComponent<PathCollider>();
        pathCollider.SetPhysicsMaterial(physicsMaterial2D);
        pathCollider.GenerateCollider();
        }

    private void SetRoadMesh()
        {
        roadMesh = GetComponent<RoadMeshCreator>();
        roadMesh.thickness = 0.15f;
        roadMesh.roadWidth = 0.09f;
        roadMesh.roadMaterial = material;
        roadMesh.undersideMaterial = material;
        roadMesh.autoUpdate = true;
        roadMesh.flattenSurface = false;
        }
    }
