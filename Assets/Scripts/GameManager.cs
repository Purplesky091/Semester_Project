using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    private int KnightCount;
    private int PeasantCount;
    public Board board;
    public GameObject KnightPrefab;
    public GameObject PeasantPrefab;

    // create the board.
    void Start()
    {
        instance = this;
        KnightCount = 0;
        PeasantCount = 0;
    }

    // Update is called once per frame
    void Update ()
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
        GameObject piecePrefab;
        if (KnightCount < 4 && tileY == 7 && getPieceAt(tileX, tileY) == null)
        {
            piecePrefab = KnightPrefab;
            ++KnightCount;
            Instantiate(piecePrefab, new Vector2(tileX + board.boardHolder.position.x, tileY + board.boardHolder.position.y), Quaternion.identity);
        }
        else if (PeasantCount < 16 && tileY < 4)
        {
            piecePrefab = PeasantPrefab;
            ++PeasantCount;
            Instantiate(piecePrefab, new Vector2(tileX + board.boardHolder.position.x, tileY + board.boardHolder.position.y), Quaternion.identity);
        }
    }

    public void DeletePlayer(float tileX, float tileY)
    {
        GameObject piece = getPieceAt(tileX, tileY);
        if (piece != null)
        {
            if (piece.name == "Knight(Clone)")
            {
                Destroy(piece);
                --KnightCount;
            }
            else
            {
                Destroy(piece);
                --PeasantCount;
            }
        }
    }

    private GameObject getPieceAt (float x, float y)
    {
        foreach(GameObject piece in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if ((piece.name == "Knight(Clone)" || piece.name == "Peasant(Clone)") && piece.transform.position.x == (x - 3.5) && piece.transform.position.y == (y - 3.5))
                return piece;
        }

        return null;
    }
}
