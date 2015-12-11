using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;

public class Board : MonoBehaviour {

    public Transform boardHolder; // the game object in the Unity hierarchy that holds the board script.
    public Transform knightHolder; // game object in unity's hierarchy that holds all the knight game objects.
    public Transform peasantHolder; // game object in unity's hierarchy that holds all the peasant game objects.
    private List<KnightRender> KnightList;
    private List<PeasantRender> PeasantList;
    private int KnightCount;
    private int PeasantCount;
    public GameObject TilePrefab;
    public GameObject KnightPrefab;
    public GameObject PeasantPrefab;
    public int boardSize = 8;
    Tile[,] map; //ON THE map, the x = column, y = row. Top row = row 7. 
    private static Vector2 boardPosition;

	// create the board.
	void Start ()
    {
        map = new Tile[boardSize, boardSize];
        KnightCount = 0;
        PeasantCount = 0;
        generateBoard();
        colorBoard();
        boardHolder.position = new Vector2(-3.5f, 3.5f); //move the whole board to the center of the screen.
        boardPosition = boardHolder.position;
    }

    //generates the board
    private void generateBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j > -boardSize; j--)
            {
                Tile tile = (Instantiate(TilePrefab, new Vector2(i, j), Quaternion.identity) as GameObject).GetComponent<Tile>();
                tile.Initialize(new Vector2Int(i, j), (-j * 10) + i);
                //tile.gridPosition = new Vector2(i, j); //sets the tile's row/column
                map[i, -j] = tile; 
                map[i, -j].transform.SetParent(boardHolder);
            }
        }
    }

    /// <summary>
    /// Creates the checker pattern by tinting
    /// </summary>
    private void colorBoard()
    {
        bool shouldSpawnDarkTile = false;

        for (int i = 0; i < map.GetLength(0); i++)
        {
            // if we're on an even row, then the first tile should be light colored.
            if (i % 2 == 0)
            {
                shouldSpawnDarkTile = false;
            }
            else
            {
                shouldSpawnDarkTile = true;
            }

            for (int j = 0; j < map.GetLength(1); j++)
            {

                if (shouldSpawnDarkTile)
                    map[i, j].MakeTileDark();

                shouldSpawnDarkTile = !shouldSpawnDarkTile; 
            }
        }
    }

    public void SetPiece(int tileX, int tileY, GameState piece)
    {
        if (piece == GameState.KNIGHT_INIT && KnightCount < 4 && tileY == 0 && map[tileX, -tileY].hasPiece() == false)
        {
            ++KnightCount;
            if (KnightCount == 4)
                AlertScript.instance.ActivateAlertBox(true, "Confirm?");

            // creates a new knight on the board and returns the knight just created.
            GameObject newKnight = Instantiate(KnightPrefab, new Vector2(tileX + boardHolder.position.x, tileY + boardHolder.position.y), Quaternion.identity) as GameObject;
            newKnight.GetComponent<KnightRender>().gridPosition = new Vector2Int(tileX, tileY);
            newKnight.transform.SetParent(knightHolder); //makes the knights a child of the empty knight game object.

            map[tileX, -tileY].setKnight(true);
            map[tileX, -tileY].ColliderSwitch(false);
        }
        else if (piece == GameState.PEASANT_INIT && PeasantCount < 16 && tileY < -3 && map[tileX, -tileY].hasPiece() == false)
        {
            ++PeasantCount;
            if (PeasantCount == 16)
                AlertScript.instance.ActivateAlertBox(true, "Confirm?");

            GameObject newPeasant = Instantiate(PeasantPrefab, new Vector2(tileX + boardHolder.position.x, tileY + boardHolder.position.y), Quaternion.identity) as GameObject;
            newPeasant.transform.SetParent(peasantHolder);
            newPeasant.GetComponent<PeasantRender>().gridPosition = new Vector2Int(tileX, tileY);

            map[tileX, -tileY].setPeasant(true);
            map[tileX, -tileY].ColliderSwitch(false);
        }
    }

    public void DeletePiece(int tileX, int tileY, GameObject piece, GameState piecePhase)
    {
        switch (piecePhase)
        {
            case GameState.KNIGHT_INIT:
                if (piece.tag == "Knight")
                {
                    KillKnight(tileX, tileY, piece);
                }
                break;

            case GameState.PEASANT_INIT:
                if (piece.tag == "Peasant")
                {
                    KillPeasant(tileX, tileY, piece);
                }
                break;

            default:
                if (piece.tag == "Knight")
                {
                    KnightList.Remove(piece.GetComponent<KnightRender>());
                    KillKnight(tileX, tileY, piece);
                }
                else if (piece.tag == "Peasant")
                {
                    PeasantList.Remove(piece.GetComponent<PeasantRender>());
                    KillPeasant(tileX, tileY, piece);
                }
                break;
        }
        /*
        if (piece.tag == "Knight" && piecePhase == GameState.KNIGHT_INIT)
        {
            Destroy(piece);
            --KnightCount;
            map[tileX, tileY].setKnight(false);
            map[tileX, tileY].ColliderSwitch(true);
        }
        else if (piece.tag == "Peasant" && piecePhase == GameState.PEASANT_INIT)
        {
            Destroy(piece);
            --PeasantCount;
            map[tileX, tileY].setPeasant(false);
            map[tileX, tileY].ColliderSwitch(true);
        }
        */

    }

    private void KillKnight(int tileX, int tileY, GameObject piece)
    {
        Destroy(piece);
        --KnightCount;
        map[tileX, -tileY].setKnight(false);
        map[tileX, -tileY].ColliderSwitch(true);
    }

    private void KillPeasant(int tileX, int tileY, GameObject piece)
    {
        Destroy(piece);
        --PeasantCount;
        map[tileX, -tileY].setPeasant(false);
        map[tileX, -tileY].ColliderSwitch(true);
    }

    public void InitKnightList()
    {
        KnightRender[] knights = knightHolder.GetComponentsInChildren<KnightRender>();
        KnightList = new List<KnightRender>(knights);
    }

    public void InitPeasantList()
    {
        PeasantRender[] peasants = peasantHolder.GetComponentsInChildren<PeasantRender>();
        PeasantList = new List<PeasantRender>(peasants);
    }

    

    public static Vector2 GridToScreenPoints(int gridX, int gridY)
    {
        Vector2 result = new Vector2(gridX + boardPosition.x, gridY + boardPosition.y);
        return result;
    }

    public static int RowFromID(int id) { return id / 10; }
    public static int ColFromID(int id) { return id % 10; }

    public int[] getKnightLocations()
    {
        int[] locationList = new int[KnightList.Count];
        for (int i = 0; i < KnightList.Count; i++)
        {
            locationList[i] = KnightList[i].tileID;
        }

        return locationList;
    }

    public int[] getPeasantLocations()
    {
        int[] locationList = new int[PeasantList.Count];
        for (int i = 0; i < PeasantList.Count; i++)
        {
            locationList[i] = PeasantList[i].tileID;
        }

        return locationList;
    }

    public void HighlightTile(int tileID)
    {
        int col = ColFromID(tileID);
        int row = RowFromID(tileID);

        map[row, col].Highlight(HighlightType.Attack);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
