using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // References
    AudioSource audioSource;

    // Player score
    public int score;
    public int highScore = 0;

    // How many times can the Player die in Level 10 before they lose the game?
    public int chances = 0;
    public int chancesUsed = 0;

    // Current level
    public string currentLevel = "Intro";

    // Which level to load as next after passing through the door?
    public string levelToLoad;

    // Whether the game should be paused
    public bool isPaused = false;

    // Volume
    public float volume;
        
    // The keys for data storage with PlayerPrefs
    private static string scoreKey = "PLAYER_SCORE";
    private static string highScoreKey = "PLAYER_HIGHSCORE";
    private static string currentLevelKey = "CURRENT_LEVEL";
    private static string chancesKey = "PLAYER_CHANCES";
    private static string chancesUsedKey = "PLAYER_CHANCES_USED";
    private static string volumeKey = "VOLUME";

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        LoadVolume();
        LoadData();
    }

    void OnDestroy()
    {
        SaveVolume();
    }       

    /// <summary>
    /// Save score, high score and current level
    /// </summary>
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

    /// <summary>
    /// Load current level, score, high score, number of used chances 
    /// and volume and calculate chances
    /// </summary>
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

        if (PlayerPrefs.HasKey(chancesUsedKey))
        {
            chancesUsed = PlayerPrefs.GetInt(chancesUsedKey);
            chances = 1 + score / 100 - chancesUsed;
            PlayerPrefs.SetInt(chancesKey, chances);
        }

        if (PlayerPrefs.HasKey(volumeKey))
        {
            volume = PlayerPrefs.GetFloat(volumeKey);
            audioSource.volume = volume;
        }
    }
        
    /// <summary>
    /// Lose one chance of repeating the last level
    /// </summary>
    public void LoseChance()
    {
        chancesUsed++;
        PlayerPrefs.SetInt(chancesUsedKey, chancesUsed);
    }

    /// <summary>
    /// Increase score
    /// </summary>
    /// <param name="amount">The amount to increase by</param>
    public void AddScore(int amount)
    {
        score += amount;
    }
       
    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    /// <summary>
    /// Resume the game
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }


    // Main Menu Actions
    
    /// <summary>
    /// Load the current level
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene(currentLevel);
    }

    /// <summary>
    /// Reset score, number of used chances and current level after the game is over
    /// </summary>
    void Reset()
    {
        score = 0;
        chancesUsed = 0;
        currentLevel = "Level1";
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.SetString(currentLevelKey, currentLevel);
        PlayerPrefs.SetInt(chancesUsedKey, chancesUsed);

        ResumeGame();
    }

    /// <summary>
    /// Reset all data except high score and start the game over from level 1
    /// </summary>
    public void NewGame()
    {
        Reset();
        SceneManager.LoadScene(currentLevel);
    }

    /// <summary>
    /// Load the Story scene
    /// </summary>
    public void ShowStory()
    {
        SceneManager.LoadScene("Story");
    }

    /// <summary>
    /// Load the Instructions scene
    /// </summary>
    public void ShowInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    /// <summary>
    /// Reset high score
    /// </summary>
    public void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.SetInt(highScoreKey, highScore);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    /// <summary>
    /// Reset the game and then quit
    /// </summary>
    public void ResetAndQuit()
    {
        Reset();
        Quit();
    }

    /// <summary>
    /// Navigate to Main Menu from other scenes
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Load sound volume
    /// </summary>
    public void LoadVolume()
    {
        if (PlayerPrefs.HasKey(volumeKey))
        {
            volume = PlayerPrefs.GetFloat(volumeKey);
            audioSource.volume = volume;
        }
    }

    /// <summary>
    /// Save sound vulume
    /// </summary>
    public void SaveVolume()
    {
        volume = audioSource.volume;
        PlayerPrefs.SetFloat(volumeKey, volume);
    }
}

