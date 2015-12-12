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
                        if (!backendLogic.IsGameRunning())
                        {
                            piecePhase = GameState.GAME_OVER;
                            break;
                        }

                        board.ClearAllKnights();
                        board.DrawKnights(backendLogic.GetKnightLocations());
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

                            if (!backendLogic.IsGameRunning())
                            {
                                piecePhase = GameState.GAME_OVER;
                                break;
                            }
                        }
                    }
                }
                else
                    piecePhase = GameState.PEASANT_UNSELECTED;
                
                break;
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

                        //redraw knights
                        board.ClearAllKnights();
                        board.DrawKnights(backendLogic.GetKnightLocations());
                        if (peasantMoveCount < 5)
                            piecePhase = GameState.PEASANT_UNSELECTED;
                        else
                            piecePhase = GameState.KNIGHT_UNSELECTED;
                        //piecePhase = GameState.KNIGHT_ATTACK;
                        backendLogic.BoardCleanup();

                        if (!backendLogic.IsGameRunning())
                        {
                            piecePhase = GameState.GAME_OVER;
                            break;
                        }
                    }
                    else
                    {
                        board.UnhighlightTiles(moveLocations);
                        backendLogic.BoardCleanup();

                        if (!backendLogic.IsGameRunning())
                        {
                            piecePhase = GameState.GAME_OVER;
                            break;
                        }
                        piecePhase = GameState.PEASANT_UNSELECTED;
                    }
                }
                break;

            case GameState.GAME_OVER:
                switch (backendLogic.GetWinCondition())
                {
                    /* 
                    * 1 = Peasants win
                    * 2 = Knight wins by killing peasants
                    * 3 = Knight wins by pillaging village
                    */

                    case 1:
                        Application.LoadLevel("PeasantWin");
                        break;

                    case 2:
                        Application.LoadLevel("KnightKillPeasantsWin");
                        break;

                    case 3:
                        Application.LoadLevel("KnightWin");
                        break;
                }
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
