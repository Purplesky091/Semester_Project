using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public PieceEnum piecePhase;
    private bool alertAnswer;
    public Board board;
    private MouseController mouseController;

    // create the board.
    void Start()
    {
        instance = this;
        alertAnswer = false;
        mouseController = GetComponent<MouseController>();
        piecePhase = PieceEnum.KNIGHT_INIT;
    }

    // Update is called once per frame
    void Update ()
    {
        switch (piecePhase)
        {
            case PieceEnum.KNIGHT_INIT:
                if (mouseController.pollForLeftClick()
                    && mouseController.lastCollidedObject.tag == "Tile")
                {
                    Tile tile = mouseController.lastCollidedObject.gameObject.GetComponent<Tile>();
                    SpawnPlayer(tile.gridPosition.x, tile.gridPosition.y);
                }

                else if (mouseController.pollForRightClick()
                    && mouseController.lastCollidedObject.tag == "Knight")
                {
                    KnightRender knight = mouseController.lastCollidedObject.gameObject.GetComponent<KnightRender>();
                    DeletePlayer(knight.gridPosition.x, knight.gridPosition.y, knight.gameObject);
                }

                //transition to the next state.
                if (checkAlertAnswer())
                {
                    piecePhase = PieceEnum.PEASANT_INIT;
                }
                break;

            //TODO - implement peasant initialization phase
            case PieceEnum.PEASANT_INIT:
                Debug.Log("We're on the peasant phase!");
                if (checkAlertAnswer())
                    piecePhase = PieceEnum.KNIGHT;
                break;

            case PieceEnum.KNIGHT:
                Debug.Log("I'm on the knight phase!");
                break;

            default:
                print("Not implemented yet");
                break;
        }

        /*
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
        */
    }

    private void processClick()
    {

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
        board.DeletePiece(tileX, tileY, piece, piecePhase);
    }

    public void recieveAlertAnswer(bool answer)
    {
        alertAnswer = answer;
    }

    //resets the alertanswer to after checking it
    private bool checkAlertAnswer()
    {
        if (alertAnswer)
        {
            bool returnVal = alertAnswer;
            alertAnswer = false;
            return returnVal;
        }

        return false;
    }
}
