using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlertScript : MonoBehaviour
{
    public static AlertScript instance;
    public Canvas AlertMenu;
    public Text AlertText;
    public Button OKButton;
    public Button YesButton;
    public Button NoButton;
    private bool active;

	// Use this for initialization
	void Start ()
    {
        instance = this;
        AlertMenu = AlertMenu.GetComponent<Canvas>();
        OKButton = OKButton.GetComponent<Button>();
        YesButton = YesButton.GetComponent<Button>();
        NoButton = NoButton.GetComponent<Button>();
        YesButton.enabled = false;
        NoButton.enabled = false;
        YesButton.gameObject.GetComponent<Text>().text = "";
        NoButton.gameObject.GetComponent<Text>().text = "";
        active = true;
	}

    public void OKPress ()
    {
        AlertMenu.enabled = false;
        active = false;
    }

    public void YESPress ()
    {
        AlertMenu.enabled = false;
        active = false;
        GameManager.instance.recieveAlertAnswer(true);
    }

    public void NOPress ()
    {
        AlertMenu.enabled = false;
        active = false;
        GameManager.instance.recieveAlertAnswer(false);
    }

    public void ActivateAlertBox (bool isYesNo, string newAlertText)
    {
        AlertText.text = newAlertText;

        if (isYesNo)
        {
            AlertMenu.enabled = true;
            OKButton.enabled = false;
            YesButton.enabled = true;
            NoButton.enabled = true;
            OKButton.gameObject.GetComponent<Text>().text = "";
            YesButton.gameObject.GetComponent<Text>().text = "YES";
            NoButton.gameObject.GetComponent<Text>().text = "NO";
            active = true;
        }
        else
        {
            AlertMenu.enabled = true;
            OKButton.enabled = true;
            YesButton.enabled = false;
            NoButton.enabled = false;
            OKButton.gameObject.GetComponent<Text>().text = "OK";
            YesButton.gameObject.GetComponent<Text>().text = "";
            NoButton.gameObject.GetComponent<Text>().text = "";
            active = true;
        }
    }

    public bool isActive()
    {
        return active;
    }
}
