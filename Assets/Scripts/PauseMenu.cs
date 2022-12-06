using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    SaveManager saveManager;
    PlayerController playerController;

    PauseMenu(SaveManager sm, PlayerController pc) {                // initialize PauseMenu with necessary SaveManager and PlayerController for playerScene and serialization
        saveManager = sm;
        playerController = pc;
    }

    public void ContinueGame() {                                    // unload the scene asynchronously, unpause time, and resume the original scene
        // Debug.Log("Continue Game");
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("PauseMenu");                 // removes the "top" scene on the additive scene overlay
    }

    public void SaveGame() {
        // Debug.Log("Save Game");
    }

    public void ExitToMainMenu() {
        // Debug.Log("Return to Main Menu");
        SceneManager.LoadScene("MainMenu");                         // returns to main menu via a scene load
    }
}
