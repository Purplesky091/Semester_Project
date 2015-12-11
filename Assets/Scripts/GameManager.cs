using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public PieceEnum piecePhase;
    private bool? alertAnswer;
    public Board board;

    // create the board.
    void Start()
    {
        instance = this;
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
        board.SetPiece(tileX, tileY, piecePhase);
    }

    public void DeletePlayer(float tileX, float tileY, GameObject piece)
    {
        if (piecePhase == PieceEnum.KNIGHT)
            board.DeletePiece(tileX, tileY, piece);
        else if (piecePhase == PieceEnum.PEASANT)
            board.DeletePiece(tileX, tileY, piece);
    }

    public void recieveAlertAnswer(bool answer)
    {
        alertAnswer = answer;
    }
}
