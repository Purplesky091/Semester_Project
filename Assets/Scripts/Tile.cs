using UnityEngine;
using Assets.Scripts;

public class Tile : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private bool hasKnight;
    private bool hasPeasant;
    private bool isDarkTile = false;
    public Vector2Int gridPosition = Vector2Int.Zero;
    //public Vector2 gridPosition = Vector2.zero;
    private BoxCollider2D boxCollider;
    public int Id;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	// Use this for initialization
	void Start () {
        
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="id"></param>
    public void Initialize(Vector2Int gridPos, int id)
    {
        gridPosition = gridPos;
        Id = id;
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
 
    public void ColliderSwitch(bool colliderOn)
    {
        boxCollider.enabled = colliderOn;
    }
}
