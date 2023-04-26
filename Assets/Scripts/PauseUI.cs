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
            
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        gm.currentLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("MainMenu");
    }

    public void NewGame()
    {
        gm.NewGame();
    }

    public void Quit()
    {
        gm.Quit();        
    }
}
