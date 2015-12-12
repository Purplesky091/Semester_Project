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

    public int tileID;

    public void MoveTo(int tileID)
    {
        Move(Board.ColFromID(tileID), Board.RowFromID(tileID));
        this.tileID = tileID;
    }

    public void Move(int x, int y)
    {
        knightTransform.position = new Vector2(y, -x) + Board.boardPosition;
    }

    void Start()
    {
        knightTransform = transform; //saves the GetComponent<>() call.
    }


}
