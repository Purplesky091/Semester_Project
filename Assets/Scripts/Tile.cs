using UnityEngine;
using Assets.Scripts;
using System.Collections;

public class Tile : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private bool hasKnight;
    private bool hasPeasant;
    private bool isDarkTile = false;
    public Vector2 gridPosition = Vector2.zero;
    private BoxCollider2D boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
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
        isDarkTile = true;
    }

    public void Highlight(HighlightType highlightType)
    {
        switch (highlightType)
        {
            case HighlightType.Attack:
                spriteRenderer.color = Constants.AttackColor;
                break;

            case HighlightType.Move:
                spriteRenderer.color = Constants.MoveColor;
                break;
        }
    }

    public void Unhighlight()
    {
        spriteRenderer.color = isDarkTile ? ColorConverter.GetUnityColor(83f, 70f, 70f) : Color.white;
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

    // tests highlighting
    void OnMouseEnter()
    {
        Highlight(HighlightType.Move);
    }

    // tests highlighting
    void OnMouseExit()
    {
        Unhighlight();
    }
 
    public void ColliderSwitch(bool colliderOn)
    {
        boxCollider.enabled = colliderOn;
    }
}
