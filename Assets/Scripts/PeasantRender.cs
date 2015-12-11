using UnityEngine;
using System.Collections;

public class PeasantRender : MonoBehaviour
{
    public Vector2 gridPosition = Vector2.zero;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
            OnRightMouseDown();
    }

    void OnRightMouseDown()
    {
        if (!AlertScript.instance.isActive())
            GameManager.instance.DeletePlayer(this.gridPosition.x, this.gridPosition.y, this.gameObject);
    }
}
