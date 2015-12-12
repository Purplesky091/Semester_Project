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
    private int[] attackLocations;
    KnightRender selectedKnight;
    PeasantRender selectedPeasant;
    int piece;
    int moveDest;
    int attack;
    int peasantMoveCount;

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
                    selectedKnight = mouseController.lastCollidedObject.gameObject.GetComponent<KnightRender>();
                    piece = selectedKnight.tileID;
                    moveLocations = backendLogic.GetMoveLocations(selectedKnight.tileID);
                    board.HighlightTiles(moveLocations);
                    piecePhase = GameState.KNIGHT_SELECTED;
                }
                break;

            case GameState.KNIGHT_SELECTED:
                if (mouseController.pollForLeftClick() && mouseController.lastCollidedObject.tag == "Tile")
                {
                    Tile tile = mouseController.lastCollidedObject.gameObject.GetComponent<Tile>();
                    if (tile.isHighlighted)
                    {
                        backendLogic.MovePiece(selectedKnight.tileID, tile.Id);
                        moveDest = tile.Id;
                        board.moveKnight(selectedKnight, tile.Id);
                        board.UnhighlightTiles(moveLocations);
                        piecePhase = GameState.KNIGHT_ATTACK;
                        attackLocations = backendLogic.GetAttackLocations(moveDest);
                        peasantMoveCount = 0;
                        backendLogic.BoardCleanup();
                        board.ClearAllKnights();
                        //board.DrawKnights(backendLogic.GetKnightLocations());
                    }
                    else
                    {
                        board.UnhighlightTiles(moveLocations);
                        piecePhase = GameState.KNIGHT_UNSELECTED;
                    }
                }

                
                break;

            case GameState.KNIGHT_ATTACK:
                if (attackLocations.Length > 0)
                {
                    board.HighlightTiles(attackLocations, HighlightType.Attack);

                    if (mouseController.pollForLeftClick() && mouseController.lastCollidedObject.tag == "Peasant")
                    {

                        PeasantRender peasant = mouseController.lastCollidedObject.gameObject.GetComponent<PeasantRender>();
                        attack = peasant.tileID;
                        print("memememe");
                        if (board.isTileHighlighted(peasant.tileID))
                        {
                            board.DeletePiece(peasant.gameObject, attack);
                            backendLogic.AttackLocation(attack);
                            board.UnhighlightTiles(attackLocations);
                            piecePhase = GameState.PEASANT_UNSELECTED;
                        }
                    }
                }
                else
                    piecePhase = GameState.PEASANT_UNSELECTED;


                break;
            /*
            int[] attackLocations = backendLogic.GetAttackLocations(moveDest);
            if (attackLocations.Length > 0)
            {
                board.HighlightTiles(attackLocations, HighlightType.Attack);
                Tile tile = mouseController.lastCollidedObject.gameObject.GetComponent<Tile>();

                if(mouseController.pollForLeftClick() && mouseController.lastCollidedObject.tag == "Peasant")
                {
                    print("deleteing peasant");
                    PeasantRender peasant = mouseController.lastCollidedObject.gameObject.GetComponent<PeasantRender>();
                    if (board.isTileHighlighted(peasant.tileID))
                    {
                        board.DeletePiece(peasant.gameObject, peasant.tileID);
                        backendLogic.AttackLocation(peasant.tileID);
                    }
                    else
                    {
                        //piecePhase = GameState.KNIGHT_UNSELECTED;
                    }
                }
            }
            */


            case GameState.PEASANT_UNSELECTED:
                if (mouseController.pollForLeftClick() && mouseController.lastCollidedObject.tag == "Peasant")
                {
                    selectedPeasant = mouseController.lastCollidedObject.gameObject.GetComponent<PeasantRender>();
                    piece = selectedPeasant.tileID;
                    moveLocations = backendLogic.GetMoveLocations(piece);
                    board.HighlightTiles(moveLocations);
                    piecePhase = GameState.PEASANT_SELECTED;
                }
  
                break;

            case GameState.PEASANT_SELECTED:
                if (mouseController.pollForLeftClick() && mouseController.lastCollidedObject.tag == "Tile")
                {
                    Tile tile = mouseController.lastCollidedObject.gameObject.GetComponent<Tile>();
                    if (tile.isHighlighted)
                    {
                        moveDest = tile.Id;
                        backendLogic.MovePiece(piece, moveDest);
                        board.movePeasant(selectedPeasant, moveDest);
                        board.UnhighlightTiles(moveLocations);
                        peasantMoveCount++;
                        if (peasantMoveCount < 6)
                            piecePhase = GameState.PEASANT_UNSELECTED;
                        else
                            piecePhase = GameState.KNIGHT_UNSELECTED;
                        //piecePhase = GameState.KNIGHT_ATTACK;
                        backendLogic.BoardCleanup();
                    }
                    else
                    {
                        board.UnhighlightTiles(moveLocations);
                        backendLogic.BoardCleanup();
                        piecePhase = GameState.PEASANT_UNSELECTED;
                    }
                }
                break;
            default:
                print("Not implemented yet");
                break;
        }
    }

    private void DoPeasantTurn()
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
