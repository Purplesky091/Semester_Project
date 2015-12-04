using UnityEngine;
using Assets.Scripts;
using System.Collections;

public class Tile : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
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

    void OnMouseEnter()
    {
        Debug.Log("I'm position (" + gridPosition.x + "," + gridPosition.y + ")");
    }
}
