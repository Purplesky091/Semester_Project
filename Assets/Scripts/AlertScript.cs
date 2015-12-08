using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlertScript : MonoBehaviour
{
    public Canvas AlertMenu;
    public Button OKButton;
    public Button YesButton;
    public Button NoButton;
    private static bool active;

	// Use this for initialization
	void Start ()
    {
        AlertMenu = AlertMenu.GetComponent<Canvas>();
        OKButton = OKButton.GetComponent<Button>();
        YesButton = YesButton.GetComponent<Button>();
        NoButton = NoButton.GetComponent<Button>();
        YesButton.enabled = false;
        NoButton.enabled = false;
        active = true;
	}

    public void OKPress ()
    {
        AlertMenu.enabled = false;
        active = false;
    }

    public void ActivateAlertBox (bool isYesNo)
    {
        if (isYesNo)
        {
            AlertMenu.enabled = true;
            OKButton.enabled = false;
            YesButton.enabled = true;
            NoButton.enabled = true;
            active = true;
        }
        else
        {
            AlertMenu.enabled = true;
            OKButton.enabled = true;
            YesButton.enabled = false;
            NoButton.enabled = false;
            active = true;
        }
    }

    public static bool isActive()
    {
        return active;
    }
}
