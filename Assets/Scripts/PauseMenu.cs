using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    SaveManager saveManager;
    PlayerController playerController;    

    public PauseMenu(SaveManager sm, PlayerController pc) {                // initialize PauseMenu with necessary SaveManager and PlayerController for playerScene and serialization
        this.saveManager = sm;
        this.playerController = pc;
    }

    public void ContinueGame() {                                    // unload the scene asynchronously, unpause time, and resume the original scene
        // Debug.Log("Continue Game");
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
        // Debug.Log("Return to Main Menu");
        SceneManager.LoadScene("MainMenu");                         // returns to main menu via a scene load
    }
}
