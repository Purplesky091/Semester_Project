using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Button single;
    public Button multi;
    public Button options;
    public Button exit;

	// Use this for initialization
	void Start ()
    {
        quitMenu = quitMenu.GetComponent<Canvas>();
        single = single.GetComponent<Button>();
        multi = multi.GetComponent<Button>();
        options = options.GetComponent<Button>();
        exit = exit.GetComponent<Button>();
        quitMenu.enabled = false;
	}

    public void ExitPress()
    {
        quitMenu.enabled = true;
        single.enabled = false;
        multi.enabled = false;
        options.enabled = false;
        exit.enabled = false;
        single.gameObject.GetComponent<Text>().color = Color.black;
        multi.gameObject.GetComponent<Text>().color = Color.black;
        options.gameObject.GetComponent<Text>().color = Color.black;
        exit.gameObject.GetComponent<Text>().color = Color.black;
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        single.enabled = true;
        multi.enabled = true;
        options.enabled = true;
        exit.enabled = true;
        single.gameObject.GetComponent<Text>().color = Color.white;
        multi.gameObject.GetComponent<Text>().color = Color.white;
        options.gameObject.GetComponent<Text>().color = Color.white;
        exit.gameObject.GetComponent<Text>().color = Color.white;
    }
	
    public void StartSingle()
    {
        Application.LoadLevel(1);
    }

    public void StartMulti()
    {
        Application.LoadLevel(2);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
