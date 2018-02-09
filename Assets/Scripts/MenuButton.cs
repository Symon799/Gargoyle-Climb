using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    //called when start button clicked
	public void buttonStartGame()
    {
        SceneManager.LoadScene("Level");
    }

    //called when exit button clicked
    public void quit()
    {
        Debug.Log("Player has quit game");
        Application.Quit();
    }
}
