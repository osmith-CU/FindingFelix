using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour                            // class to house the functionality of buttons for EndGameMenu scene
{
    public void ExitToMainMenu() {
        SceneManager.LoadScene("MainMenu");                         // returns to main menu via a scene load
    }
}
