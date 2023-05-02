using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    // References
    private GameManager gm;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
            
    /// <summary>
    /// Restart level by loading the active scene again
    /// </summary>
    public void RestartLevel()
    {
        gm.ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Remember the current scene and then navigate to the Main Menu
    /// </summary>
    public void MainMenu()
    {
        gm.currentLevel = SceneManager.GetActiveScene().name;
        gm.ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Start the game over
    /// </summary>
    public void NewGame()
    {
        gm.NewGame();
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void Quit()
    {
        gm.Quit();        
    }
}
