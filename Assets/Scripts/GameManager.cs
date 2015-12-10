using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private int KnightCount;
    private int PeasantCount;
    private enum PieceEnum
    {
        KNIGHT,
        PEASANT
    }
    private PieceEnum piecePhase;
    private bool? alertAnswer;
    public Board board;
    public GameObject KnightPrefab;
    public GameObject PeasantPrefab;

    // create the board.
    void Start()
    {
        instance = this;
        KnightCount = 0;
        PeasantCount = 0;
        alertAnswer = null;
        piecePhase = PieceEnum.KNIGHT;
    }

    // Update is called once per frame
    void Update ()
    {
        if (alertAnswer == true && piecePhase == PieceEnum.KNIGHT)
        {
            piecePhase = PieceEnum.PEASANT;
            AlertScript.instance.ActivateAlertBox(false, "Peasant player, please arrange your sixteen pieces in the bottom four rows.");
        }
        else if (alertAnswer == true && piecePhase == PieceEnum.PEASANT)
        {
            piecePhase = PieceEnum.KNIGHT;
            AlertScript.instance.ActivateAlertBox(false, "The game may now begin!");
        }

        alertAnswer = null;
    }

    /// <summary>
    /// Spawns a piece at the specified tile.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="piecePrefab">This is the prefab you wish to spawn on a tile. Could be a knight or a peasant.</param>
    public void SpawnPlayer(float tileX, float tileY)
    {
        if (KnightCount < 4 && tileY == 7 && getPieceAt(tileX, tileY) == null && piecePhase == PieceEnum.KNIGHT)
        {
            SetPiece(tileX, tileY, KnightPrefab);
            ++KnightCount;
            if (KnightCount == 4)
                AlertScript.instance.ActivateAlertBox(true, "Confirm?");
        }
        else if (PeasantCount < 16 && tileY < 4 && getPieceAt(tileX, tileY) == null && piecePhase == PieceEnum.PEASANT)
        {
            SetPiece(tileX, tileY, PeasantPrefab);
            ++PeasantCount;
            if (PeasantCount == 16)
                AlertScript.instance.ActivateAlertBox(true, "Confirm?");
        }
    }
    private void SetPiece(float tileX, float tileY, GameObject piecePrefab)
    {
        Instantiate(piecePrefab, new Vector2(tileX + board.boardHolder.position.x, tileY + board.boardHolder.position.y), Quaternion.identity);
    }

    public void DeletePlayer(float tileX, float tileY, GameObject piece)
    {
        if (piece.name == "Knight(Clone)" && piecePhase == PieceEnum.KNIGHT)
        {
            Destroy(piece);
            --KnightCount;
        }
        else if (piece.name == "Peasant(Clone)" && piecePhase == PieceEnum.PEASANT)
        {
            Destroy(piece);
            --PeasantCount;
        }
    }

    public void recieveAlertAnswer(bool answer)
    {
        alertAnswer = answer;
    }

    private GameObject getPieceAt(float x, float y)
    {
        foreach(GameObject piece in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if ((piece.name == "Knight(Clone)" || piece.name == "Peasant(Clone)") && piece.transform.position.x == (x + board.boardHolder.position.x) 
             && piece.transform.position.y == (y + board.boardHolder.position.y))
                return piece;
        }

        return null;
    }
}
