using UnityEngine;
using Assets.Scripts;
using System.Collections;

public class Tile : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private bool hasKnight;
    private bool hasPeasant;
    public Vector2 gridPosition = Vector2.zero;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        	
	}

    /// <summary>
    /// Makes the tile the dark wood color.
    /// </summary>
    public void MakeTileDark()
    {
        spriteRenderer.color = ColorConverter.GetUnityColor(83f, 70f, 70f);
    }

    public bool hasPiece()
    {
        if (hasKnight || hasPeasant)
            return true;
        else
            return false;
    }

    public void setKnight(bool hasKnight)
    {
        this.hasKnight = hasKnight;
    }

    public void setPeasant(bool hasPeasant)
    {
        this.hasPeasant = hasPeasant;
    }

    void OnMouseEnter()
    {
       // Debug.Log("I'm position (" + gridPosition.x + "," + gridPosition.y + ")");
    }
 
    void OnMouseDown()
    {
        if (!AlertScript.instance.isActive())
            GameManager.instance.SpawnPlayer(this.gridPosition.x, this.gridPosition.y);
    }
}
