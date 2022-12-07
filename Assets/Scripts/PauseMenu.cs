using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    SaveManager saveManager;

    public PauseMenu(SaveManager sm) {                              // initialize PauseMenu with necessary SaveManager and PlayerController for playerScene and serialization
        saveManager = sm;
    }

    public void ContinueGame() {                                    // unload the scene asynchronously, unpause time, and resume the original scene
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("PauseMenu");                 // removes the "top" scene on the additive scene overlay
    }

    public void SaveGame() {
        saveManager = new SaveManager("Save_0");
        saveManager.updateStage(SceneManager.GetSceneAt(0).name);
        Debug.Log(SceneManager.GetSceneAt(0).name);
        saveManager.save();
    }

    public void ExitToMainMenu() {
        SceneManager.LoadScene("MainMenu");                         // returns to main menu via a scene load
    }
}
