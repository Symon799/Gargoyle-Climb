using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    //called when button clicked
	public void buttonStartGame()
    {
        SceneManager.LoadScene("Level");
    }
}
