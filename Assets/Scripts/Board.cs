using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

    public Transform boardHolder; // the game object in the Unity hierarchy that holds the board script.
    public GameObject TilePrefab;
    public GameObject KnightPrefab;
    public int boardSize = 8;
    Tile[,] map; //ON THE map, the x = column, y = row. Top row = row 7. 

	// create the board.
	void Start () {
        map = new Tile[boardSize, boardSize]; 
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

	// Update is called once per frame
	void Update () {
	
	}
}
