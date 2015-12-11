using UnityEngine;
using Assets.Scripts;

public class GameManager : MonoBehaviour {
   // public static GameManager instance;
    public GameState piecePhase;
    private static bool alertAnswer;
    public Board board;
    private MouseController mouseController;

    // create the board.
    void Start()
    {
       // instance = this;
        alertAnswer = false;
        mouseController = GetComponent<MouseController>();
        piecePhase = GameState.KNIGHT_INIT;
    }

    // Update is called once per frame
    void Update ()
    {
        switch (piecePhase)
        {
            case GameState.KNIGHT_INIT:
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
                    board.InitKnightList();
                    AlertScript.instance.ActivateAlertBox(false, "Peasant player, please arrange your sixteen pieces in the bottom four rows.");
                    piecePhase = GameState.PEASANT_INIT;
                }
                break;

            //TODO - implement peasant initialization phase
            case GameState.PEASANT_INIT:
                if (mouseController.pollForLeftClick()
                    && mouseController.lastCollidedObject.tag == "Tile")
                {
                    Tile tile = mouseController.lastCollidedObject.gameObject.GetComponent<Tile>();
                    SpawnPlayer(tile.gridPosition.x, tile.gridPosition.y);
                }

                else if (mouseController.pollForRightClick()
                    && mouseController.lastCollidedObject.tag == "Peasant")
                {
                    PeasantRender peasant = mouseController.lastCollidedObject.gameObject.GetComponent<PeasantRender>();
                    DeletePlayer(peasant.gridPosition.x, peasant.gridPosition.y, peasant.gameObject);
                }

                //transition to the next state.
                if (checkAlertAnswer())
                {
                    board.InitPeasantList();
                    AlertScript.instance.ActivateAlertBox(false, "Let the games begin!");
                    piecePhase = GameState.KNIGHT;
                }
                break;

            case GameState.KNIGHT:
                Debug.Log("I'm on the knight phase!");
                break;

            default:
                print("Not implemented yet");
                break;
        }
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
    public void SpawnPlayer(int tileX, int tileY)
    {
        board.SetPiece(tileX, tileY, piecePhase);
    }

    public void DeletePlayer(int tileX, int tileY, GameObject piece)
    {
        board.DeletePiece(tileX, tileY, piece, piecePhase);
    }

    public static void recieveAlertAnswer(bool answer)
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
