using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This object acts as a caretaker for our memento pattern
public class MainMenu : MonoBehaviour {
    private SaveManager saveManager;
    public void StartGame() {
        SceneManager.LoadScene("Level1");
    }

    public void LoadGame(){             
        Debug.Log("here");
        saveManager = new SaveManager("Save_0");
        //declares a new save manager
        saveManager.load();
    }

    public void ExitGame() {
        Application.Quit();
    }
}
