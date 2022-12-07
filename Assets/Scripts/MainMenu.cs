using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This object acts as a caretaker for our memento pattern

public class MainMenu : MonoBehaviour {
    private SaveManager saveManager;
    public void StartGame() {                   // loads in the first level in the build order
        SceneManager.LoadScene("Level1");
    }

    public void LoadGame(){                     // loads from the persistent file path
        Debug.Log("here");
        saveManager = new SaveManager("Save_0");
        //declares a new save manager
        saveManager.load();
    }

    public void ExitGame() {                    // exits the application
        Application.Quit();
    }
}
