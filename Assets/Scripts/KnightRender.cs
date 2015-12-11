using UnityEngine;
using System.Collections;

public class KnightRender : MonoBehaviour
{
    public Vector2 gridPosition = Vector2.zero;
    private Transform knightTransform;

    void Start()
    {
        knightTransform = transform; //saves the GetComponent<>() call.
    }

    public void Move(float x, float y)
    {
        knightTransform.position = Board.GridToScreenPoints(x, y);
    }
}
