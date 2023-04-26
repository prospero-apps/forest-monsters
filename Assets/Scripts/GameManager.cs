using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Player score
    public int score;
    public int highScore = 0;

    // Current level
    public string currentLevel = "Intro";

    // Which level to load as next after passing through the door?
    public string levelToLoad;

    // Whether the game should be paused
    public bool isPaused = false;
        
    // The keys
    private static string scoreKey = "PLAYER_SCORE";
    private static string highScoreKey = "PLAYER_HIGHSCORE";
    private static string currentLevelKey = "CURRENT_LEVEL";

    void Start()
    {
        LoadData();
    }
      
    // Save data
    public void SaveData()
    {
        PlayerPrefs.SetInt(scoreKey, score);

        if (score > highScore)
        {
            highScore = score;
        }

        PlayerPrefs.SetInt(highScoreKey, highScore);
        PlayerPrefs.SetString(currentLevelKey, levelToLoad);
    }

    // Load data
    public void LoadData()
    {
        if (PlayerPrefs.HasKey(currentLevelKey))
        {
            currentLevel = PlayerPrefs.GetString(currentLevelKey);
        }

        if (PlayerPrefs.HasKey(scoreKey))
        {
            score = PlayerPrefs.GetInt(scoreKey);
        }

        if (PlayerPrefs.HasKey(highScoreKey))
        {
            highScore = PlayerPrefs.GetInt(highScoreKey);
        }
    }

    // Increase score
    public void AddScore(int amount)
    {
        score += amount;
    }

    // Pause the game
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    // Resume the game
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }


    // Main Menu Actions       
    public void Play()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void NewGame()
    {
        score = 0;
        currentLevel = "Level1";
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.SetString(currentLevelKey, currentLevel);
        SceneManager.LoadScene(currentLevel);
    }

    public void ShowStory()
    {
        SceneManager.LoadScene("Story");
    }

    public void ShowInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetInt(highScoreKey, highScore);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    // Navigation to Main Menu from other scenes
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

