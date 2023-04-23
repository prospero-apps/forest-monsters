using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    // References
    private PlayerController player;

    // Player score
    private int score;
    private int highScore = 0;

    // Current level
    private string currentLevel = "Level1";
   
    // Which level to load as next after passing through the door?
    public string levelToLoad;

    // Whether the game should be paused
    public bool isPaused = false;

    // GUI elements
    [SerializeField] private Sprite[] healthSprites;
    [SerializeField] private Image healthUI;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    public TMP_Text doorText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject infoMenu;

    // The keys
    private static string scoreKey = "PLAYER_SCORE";
    private static string highScoreKey = "PLAYER_HIGHSCORE";
    private static string currentLevelKey = "CURRENT_LEVEL";

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        pauseMenu.SetActive(false);
        LoadData();
        StartCoroutine(HideInfoMenu());        
    }

    void Update()
    {
        // The Player's current health is an int between 0 and 5, so let's use it as the index.
        healthUI.sprite = healthSprites[player.currentHealth];

        // Update the Player's ammo and score in the UI
        ammoText.text = "Ammo: " + player.currentAmmo;
        scoreText.text = "Score: " + score;

        // Pause or resume the game if the Pause button is pressed
        if (Input.GetButtonDown("PauseOrResume") && !infoMenu.activeInHierarchy)
        {
            PauseOrResume();
        }
    }

    void OnApplicationQuit()
    {
        //SaveData();
    }

    // The level info should disappear after a while
    IEnumerator HideInfoMenu()
    {
        PauseGame();
        yield return new WaitForSecondsRealtime(3);
        infoMenu.SetActive(false);
        ResumeGame();
    }

    void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
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
            scoreText.text = "Score: " + score;
        }

        if (PlayerPrefs.HasKey(highScoreKey))
        {
            highScore = PlayerPrefs.GetInt(highScoreKey);

            if (highScore > 0)
            {
                highScoreText.text = "High Score: " + highScore;
            }
        }
    }    

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

    // Pause if not paused or resume if paused
    public void PauseOrResume()
    {
        if (isPaused)
        {
            ResumeGame();
            HidePauseMenu();
        }
        else
        {
            PauseGame();
            ShowPauseMenu();
        }
    }
}
