using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class Board : MonoBehaviour {

    public Transform boardHolder; // the game object in the Unity hierarchy that holds the board script.
    public Transform knightHolder; // game object in unity's hierarchy that holds all the knight game objects.
    public Transform peasantHolder; // game object in unity's hierarchy that holds all the peasant game objects.
//    private List<KnightRender> KnightList;
//    private List<PeasantRender> PeasantList;
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
       // KnightList = new List<KnightRender>();
       // PeasantList = new List<PeasantRender>();
        KnightCount = 0;
        PeasantCount = 0;
        generateBoard();
        colorBoard();
        boardHolder.position = new Vector2(-3.5f, -3.5f); //move the whole board to the center of the screen.
        boardPosition = boardHolder.position;
    }

    //generates the board
    private void generateBoard()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                Tile tile = (Instantiate(TilePrefab, new Vector2(i, j), Quaternion.identity) as GameObject).GetComponent<Tile>();
                tile.gridPosition = new Vector2(i, j); //sets the tile's row/column
                map[i, j] = tile; 
                map[i, j].transform.SetParent(boardHolder);
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

    public void SetPiece(float tileX, float tileY, PieceEnum piece)
    {
        if (piece == PieceEnum.KNIGHT_INIT && KnightCount < 4 && tileY == 7 && map[(int)tileX, (int)tileY].hasPiece() == false)
        {
            ++KnightCount;
            if (KnightCount == 4)
                AlertScript.instance.ActivateAlertBox(true, "Confirm?");

            // creates a new knight on the board and returns the knight just created.
            GameObject newKnight = Instantiate(KnightPrefab, new Vector2(tileX + boardHolder.position.x, tileY + boardHolder.position.y), Quaternion.identity) as GameObject;
            //KnightList.Add(newKnight);
            newKnight.GetComponent<KnightRender>().gridPosition = new Vector2(tileX, tileY);
            newKnight.transform.SetParent(knightHolder); //makes the knights a child of the empty knight game object.

            map[(int)tileX, (int)tileY].setKnight(true);
            map[(int)tileX, (int)tileY].ColliderSwitch(false);
        }
        else if (piece == PieceEnum.PEASANT_INIT && PeasantCount < 16 && tileY < 4 && map[(int) tileX, (int) tileY].hasPiece() == false)
        {
            ++PeasantCount;
            if (PeasantCount == 16)
                AlertScript.instance.ActivateAlertBox(true, "Confirm?");


            GameObject newPeasant = Instantiate(PeasantPrefab, new Vector2(tileX + boardHolder.position.x, tileY + boardHolder.position.y), Quaternion.identity) as GameObject;
            //PeasantList.Add(newPeasant);
            newPeasant.GetComponent<PeasantRender>().gridPosition = new Vector2(tileX, tileY);
            newPeasant.transform.SetParent(peasantHolder);

            map[(int)tileX, (int)tileY].setPeasant(true);
            map[(int)tileX, (int)tileY].ColliderSwitch(false);
        }
    }

    public void DeletePiece(float tileX, float tileY, GameObject piece, PieceEnum piecePhase)
    {
        if (piece.tag == "Knight" && piecePhase == PieceEnum.KNIGHT_INIT)
        {
            Destroy(piece);
            --KnightCount;
            map[(int)tileX, (int)tileY].setKnight(false);
            map[(int)tileX, (int)tileY].ColliderSwitch(true);
        }
        else if (piece.tag == "Peasant" && piecePhase == PieceEnum.PEASANT_INIT)
        {
            Destroy(piece);
            --PeasantCount;
            map[(int)tileX, (int)tileY].setPeasant(false);
            map[(int)tileX, (int)tileY].ColliderSwitch(true);
        }
    }

    public static Vector2 GridToScreenPoints(float gridX, float gridY)
    {
        Vector2 result = new Vector2(gridX + boardPosition.x, gridY + boardPosition.y);
        return result;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
