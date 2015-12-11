using UnityEngine;
using Assets.Scripts;
using Pillage;

public class GameManager : MonoBehaviour {
   // public static GameManager instance;
    public GameState piecePhase;
    private static bool alertAnswer;
    public Board board;
    private MouseController mouseController;
    BackendLogic backendLogic;
    private int[] moveLocations;

    // create the board.
    void Start()
    {
       // instance = this;
        alertAnswer = false;
        mouseController = GetComponent<MouseController>();
        piecePhase = GameState.KNIGHT_INIT;
        backendLogic = new BackendLogic();
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
                    backendLogic.PlaceStartingKnights(board.getKnightLocations());
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
                    backendLogic.PlaceStartingPeasants(board.getPeasantLocations());
                    AlertScript.instance.ActivateAlertBox(false, "Let the games begin!");
                    piecePhase = GameState.KNIGHT_UNSELECTED;
                }
                break;

            case GameState.KNIGHT_UNSELECTED:
                if (mouseController.pollForLeftClick() && mouseController.lastCollidedObject.tag == "Knight")
                {
                    KnightRender knight = mouseController.lastCollidedObject.gameObject.GetComponent<KnightRender>();
                    moveLocations = backendLogic.GetMoveLocations(knight.tileID);
                    board.HighlightTiles(moveLocations);
                }
                break;


            case GameState.PEASANT:
               /* if (mouseController.pollForLeftClick()
                    && mouseController.lastCollidedObject.tag == "Peasant")
                {
                    Tile tile = mouseController.lastCollidedObject.gameObject.GetComponent<Tile>();
                    int[] peasantLocations = backendLogic.GetPeasantLocations();
                    foreach(int peasantLocation in peasantLocations)
                    {
                        int row = Board.RowFromID(peasantLocation);
                        int col = Board.ColFromID(peasantLocation);
                        if (row == tile.gridPosition.y && col == tile.gridPosition.x)
                            tile.Highlight(HighlightType.Move);
                    }

                }*/
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
