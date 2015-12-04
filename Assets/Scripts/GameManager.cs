using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Board board;
    public GameObject KnightPrefab;
    

    // create the board.
    void Start()
    {
        float row = 7;
        for (int col = 0; col < board.boardSize; col++)
        {
            SpawnPlayer(KnightPrefab, col, row);
        }

    }

    // Update is called once per frame
    void Update () {
	
	}

    /// <summary>
    /// Spawns a piece at the specified tile.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="piecePrefab">This is the prefab you wish to spawn on a tile. Could be a knight or a peasant.</param>
    public void SpawnPlayer(GameObject piecePrefab, float tileX, float tileY)
    {
        Instantiate(piecePrefab, new Vector2(tileX + board.boardHolder.position.x, tileY + board.boardHolder.position.y), Quaternion.identity);
    }
}
