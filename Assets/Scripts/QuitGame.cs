using UnityEngine;
using System.Collections;

/// <summary>
/// Script that allows the the "quit" button to 
/// quit the game.
/// </summary>
public class QuitGame : MonoBehaviour {

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void Quit()
    {
        // stops the editor
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit(); //quits the applications. Ignored by the editor.
    }
}
