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
    //public static float originalVolume;
        
    // The keys
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
        
    public void LoseChance()
    {
        chancesUsed++;
        PlayerPrefs.SetInt(chancesUsedKey, chancesUsed);
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

    // Reset the game after the game is over
    void Reset()
    {
        score = 0;
        chancesUsed = 0;
        currentLevel = "Level1";
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.SetString(currentLevelKey, currentLevel);
        PlayerPrefs.SetInt(chancesUsedKey, chancesUsed);
    }

    public void NewGame()
    {
        Reset();
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

    public void ResetAndQuit()
    {
        Reset();
        Quit();
    }

    // Navigation to Main Menu from other scenes
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Sound
    public void LoadVolume()
    {
        if (PlayerPrefs.HasKey(volumeKey))
        {
            volume = PlayerPrefs.GetFloat(volumeKey);
            audioSource.volume = volume;
        }
    }

    public void SaveVolume()
    {
        volume = audioSource.volume;
        //volume = originalVolume;
        PlayerPrefs.SetFloat(volumeKey, volume);
    }

    //public static IEnumerator FadeSound(AudioSource audioSource, float duration, float targetVolume)
    //{
    //    originalVolume = audioSource.volume;

    //    float currentTime = 0;
    //    float startVolume = audioSource.volume;

    //    while (currentTime < duration)
    //    {
    //        currentTime += Time.deltaTime;
    //        audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
    //        yield return null;
    //    }

    //    yield break;
    //}
}

