using UnityEngine;
using Assets.Scripts;
using System.Collections;

public class KnightRender : MonoBehaviour
{
    public Vector2Int gridPosition = Vector2Int.Zero;
    private Transform knightTransform;

    void Start()
    {
        knightTransform = transform; //saves the GetComponent<>() call.
    }

    public void Move(int x, int y)
    {
        knightTransform.position = Board.GridToScreenPoints(x, y);
    }
}
