using UnityEngine;
using Assets.Scripts;

public class PeasantRender : MonoBehaviour
{
    public Vector2Int gridPosition = Vector2Int.Zero;
    private Transform peasantTransform;
    public int tileID
    {
        get
        {
            return ((gridPosition.x * 10) + gridPosition.y);
        }
    }

    void Start()
    {
        peasantTransform = transform;
    }

    public void MoveTo(int tileID)
    {
        Move(Board.ColFromID(tileID), Board.RowFromID(tileID));
    }

    public void Move(int x, int y)
    {
        peasantTransform.position = Board.GridToScreenPoints(x, y);
    }
}
