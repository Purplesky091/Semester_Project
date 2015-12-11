using UnityEngine;
using System.Collections;

public class PeasantRender : MonoBehaviour
{
    public Vector2 gridPosition = Vector2.zero;
    private Transform peasantTransform;

    void Start()
    {
        peasantTransform = transform;
    }

    public void Move(float x, float y)
    {
        peasantTransform.position = Board.GridToScreenPoints(x, y);
    }
}
