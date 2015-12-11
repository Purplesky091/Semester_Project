using UnityEngine;
using Assets.Scripts;
using System.Collections;

public class PeasantRender : MonoBehaviour
{
    public Vector2Int gridPosition = Vector2Int.Zero;
    private Transform peasantTransform;

    void Start()
    {
        peasantTransform = transform;
    }

    public void Move(int x, int y)
    {
        peasantTransform.position = Board.GridToScreenPoints(x, y);
    }
}
