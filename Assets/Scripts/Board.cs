using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;

public class Board : MonoBehaviour {

    public Transform boardHolder; // the game object in the Unity hierarchy that holds the board script.
    private List<GameObject> KnightList;
    private List<GameObject> PeasantList;
    private int KnightCount;
    private int PeasantCount;
    public GameObject TilePrefab;
    public GameObject KnightPrefab;
    public GameObject PeasantPrefab;
    public int boardSize = 8;
    Tile[,] map; //ON THE map, the x = column, y = row. Top row = row 7. 

	// create the board.
	void Start () {
        map = new Tile[boardSize, boardSize];
        KnightList = new List<GameObject>();
        PeasantList = new List<GameObject>();
        KnightCount = 0;
        PeasantCount = 0;
        generateBoard();
        colorBoard();
        boardHolder.position = new Vector2(-3.5f, -3.5f); //move the whole board to the center of the screen.
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

    /*
    private void generatePlayers()
    {
        GameObject instance = Instantiate(KnightPrefab, new Vector2(0 - Mathf.Floor(boardSize / 2) + boardHolder.position.x, 0 - Mathf.Floor(boardSize / 2) + boardHolder.position.y), Quaternion.identity) as GameObject;
    }
    */
    public void SetPiece(float tileX, float tileY, PieceEnum piece)
    {
        if (piece == PieceEnum.KNIGHT && KnightCount < 4 && tileY == 7 && map[(int)tileX, (int)tileY].hasPiece() == false)
        {
            ++KnightCount;
            if (KnightCount == 4)
                AlertScript.instance.ActivateAlertBox(true, "Confirm?");
            GameObject newKnight = Instantiate(KnightPrefab, new Vector2(tileX + boardHolder.position.x, tileY + boardHolder.position.y), Quaternion.identity) as GameObject;
            KnightList.Add(newKnight);
            newKnight.GetComponent<KnightRender>().gridPosition = new Vector2(tileX, tileY);
            map[(int)tileX, (int)tileY].setKnight(true);
        }
        else if (PeasantCount < 16 && tileY < 4 && map[(int) tileX, (int) tileY].hasPiece() == false)
        {
            ++PeasantCount;
            if (PeasantCount == 16)
                AlertScript.instance.ActivateAlertBox(true, "Confirm?");
            GameObject newPeasant = Instantiate(PeasantPrefab, new Vector2(tileX + boardHolder.position.x, tileY + boardHolder.position.y), Quaternion.identity) as GameObject;
            PeasantList.Add(newPeasant);
            newPeasant.GetComponent<PeasantRender>().gridPosition = new Vector2(tileX, tileY);
            map[(int)tileX, (int)tileY].setPeasant(true);
        }
    }

    public void DeletePiece(float tileX, float tileY, GameObject piece)
    {
        if (piece.tag == "Knight")
        {
            Destroy(piece);
            --KnightCount;
            map[(int)tileX, (int)tileY].setKnight(false);
        }
        else if (piece.tag == "Peasant")
        {
            Destroy(piece);
            --PeasantCount;
            map[(int)tileX, (int)tileY].setPeasant(false);
        }
    }

    /*private GameObject getPieceAt(float x, float y)
    {
        if (PieceList.Count > 0)
        {
            foreach (GameObject piece in PieceList)
            {
                if ((piece.tag == "Knight" || piece.tag == "Peasant") && piece.transform.position.x == (x + boardHolder.position.x)
                 && piece.transform.position.y == (y + boardHolder.position.y))
                    return piece;
            }
        }

        return null;
    }*/

    // Update is called once per frame
    void Update () {
	
	}
}
