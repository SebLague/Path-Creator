using UnityEngine;
using Sirenix.OdinInspector;
using PathCreation;
using UnityEngine.U2D;

/// Uses Odin Inspector, simply calls functions from the PathToSpriteShape class via a button in the editor.
[RequireComponent(typeof(PathCreator))]
[RequireComponent(typeof(SpriteShapeController))]
public class ConvertToSpriteShape : MonoBehaviour
{
    int j = 0;
    [Button("Convert to sprite shape")] public void Convert()
        {
        Clear();
        j = 0;
        PathCreator pathCreator = GetComponent<PathCreator>();
        pathCreator.bezierPath.ControlPointMode = BezierPath.ControlMode.Mirrored;
        j = PathToSpriteShape.UpdateSpriteShape(GetComponent<SpriteShapeController>(), pathCreator, j);
        }
    [Button("Clear sprite shape")]
    public void Clear()
        {
       PathToSpriteShape.Clear(GetComponent<SpriteShapeController>());
        }
    }
