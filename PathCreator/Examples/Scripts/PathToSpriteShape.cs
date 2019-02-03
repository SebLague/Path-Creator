using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using PathCreation;


public static class PathToSpriteShape
{
    const float SCALE = 4.25f;

    /// <summary>
    /// Updates a sprite shape.
    /// </summary>
    /// <param name="controller">Sprite Shape Controller</param>
    /// <param name="pathCreator">Path Creator</param>
    /// <param name="j">Index of last bezier path segment updated</param>
    public static int UpdateSpriteShape(SpriteShapeController controller, PathCreator pathCreator, int j)
        {
        Spline spline = controller.spline;
        spline.isOpenEnded = !pathCreator.path.isClosedLoop;
        int numSegments = pathCreator.bezierPath.NumSegments;
        int index = spline.GetPointCount();
        Vector3[] segment;
        if(index == 2)
            {
            spline.Clear();
            index = 0;
            segment = pathCreator.bezierPath.GetPointsInSegment(0);
            InsertPointBezier(spline, segment[0], index);
            index++;
            InsertPointBezier(spline, segment[3], index);
            index++;
            }
        for (int i = j + 1; i < numSegments; i++)
            {
            segment = pathCreator.bezierPath.GetPointsInSegment(i);
            InsertPointBezier(spline, segment[3], index);
            index++;
            j = i;
            }

        for (int i = 1; i < spline.GetPointCount() - 1; i++)
            {
            Vector3 leftTangent, rightTangent;
            SplineUtility.CalculateTangents(spline.GetPosition(i), spline.GetPosition(i - 1), spline.GetPosition(i + 1), Vector2.right, SCALE, out rightTangent, out leftTangent);
            spline.SetTangentMode(i, ShapeTangentMode.Continuous);
            spline.SetLeftTangent(i, leftTangent);
            spline.SetRightTangent(i, rightTangent);
            }

            
        return j;
        }



    /// <summary>
    /// Generates a sprite shape from the path. BUGGY when updating the spriteshape later on with UpdateSpriteShape, misses the first point. Just use UpdateSpriteShape.
    /// </summary>
    /// <param name="controller">Sprite Shape Controller</param>
    /// <param name="pathCreator">Path Creator</param>
    public static void GenerateSpriteShape(SpriteShapeController controller, PathCreator pathCreator, out int j)
        {
        Spline spline = controller.spline;
        spline.Clear();
        spline.isOpenEnded = !pathCreator.path.isClosedLoop;
        int segments = pathCreator.bezierPath.NumSegments;
        int index = spline.GetPointCount();
        Vector3[] segment = pathCreator.bezierPath.GetPointsInSegment(0);
        InsertPointBezier(spline, segment[0], index);
        index++;
        for (j = 1; j < segments; j++)
            {
            segment = pathCreator.bezierPath.GetPointsInSegment(j);
            InsertPointBezier(spline, segment[3], index);
            index++;
            }
        for (int i = 1; i < spline.GetPointCount() - 1; i++)
            {
            Vector3 leftTangent, rightTangent;
            SplineUtility.CalculateTangents(spline.GetPosition(i), spline.GetPosition(i - 1), spline.GetPosition(i + 1), Vector2.right, SCALE, out rightTangent, out leftTangent);
            spline.SetLeftTangent(i, leftTangent);
            spline.SetRightTangent(i, rightTangent);
            }
        }

    public static void RemovePoints()
        {
        throw new System.NotImplementedException();
        }
    private static void InsertPointBezier(Spline spline, Vector3 point, int index)
        {
        spline.InsertPointAt(index, point);
        spline.SetTangentMode(index, ShapeTangentMode.Continuous);
        }

    }
