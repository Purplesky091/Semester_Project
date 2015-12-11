using UnityEngine;
using Assets.Scripts;

public class KnightRender : MonoBehaviour
{
    /// <summary>
    /// The grid positions it holds are
    /// x = column
    /// y = row.
    /// </summary>
    public Vector2Int gridPosition = Vector2Int.Zero;
    private Transform knightTransform;

    public int tileID
    {
        get
        {
            return ((gridPosition.x * 10) + gridPosition.y);
        }
    }

    public void MoveTo(int tileID)
    {
        Move(Board.ColFromID(tileID), Board.RowFromID(tileID));
    }

    void Start()
    {
        knightTransform = transform; //saves the GetComponent<>() call.
    }

    public void Move(int x, int y)
    {
        knightTransform.position = Board.GridToScreenPoints(x, y);
    }


}
