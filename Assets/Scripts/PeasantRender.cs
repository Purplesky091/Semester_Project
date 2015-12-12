using UnityEngine;
using Assets.Scripts;

public class PeasantRender : MonoBehaviour
{
    public Vector2Int gridPosition = Vector2Int.Zero;
    private Transform peasantTransform;

    void Start()
    {
        peasantTransform = transform;
    }

    public int tileID;

    public void MoveTo(int tileID)
    {
        Move(Board.ColFromID(tileID), Board.RowFromID(tileID));
        this.tileID = tileID;
    }

    public void Move(int x, int y)
    {
        peasantTransform.position = new Vector2(y, -x) + Board.boardPosition;
    }

}
