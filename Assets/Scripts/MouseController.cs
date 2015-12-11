using UnityEngine;

public class MouseController : MonoBehaviour {

    public Collider2D lastCollidedObject { get; private set; }
    //private bool wasLeftClicked = false;
    //private bool wasRightClicked = false;

	// Use this for initialization
	void Start () {
	
	}

    public bool pollForLeftClick()
    {
        return pollForClick(Input.GetMouseButtonDown(0));
    }

    public bool pollForRightClick()
    {
        return pollForClick(Input.GetMouseButtonDown(1));
    }

    private bool pollForClick(bool click)
    {
        if (AlertScript.instance.isActive())
            return false;

        if (click)
        {
            Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hitInfo = Physics2D.Raycast(mousePosition, Vector2.zero, 0f);
            if (hitInfo)
            {
                lastCollidedObject = hitInfo.collider;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

}
