using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    // References
    private GameManager gm;
    private TextMeshProUGUI chancesInfo;

    // Menus
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject infoMenu;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        StartCoroutine(HideInfoMenu());

        // If this is Level 10, inform the Player how many attempts they still have to kill the Sorcerer.
        if (SceneManager.GetActiveScene().name == "Level10")
        {
            chancesInfo = GameObject.FindGameObjectWithTag("LivesInfo").GetComponent<TextMeshProUGUI>();
            chancesInfo.text = gm.chances == 1 ? 
                "This is your last chance to kill the Sorcerer!" :
                $"You have {gm.chances} chances to kill the Sorcerer.";
        }
    }
        
    void Update()
    {
        // Pause or resume the game if the Pause button is pressed
        if (Input.GetButtonDown("Pause") && !infoMenu.activeInHierarchy)
        {
            PauseOrResume();
        }
    }

    // The level info should disappear after a while
    IEnumerator HideInfoMenu()
    {
        yield return new WaitForSeconds(1);
        gm.PauseGame();
        yield return new WaitForSecondsRealtime(8);
        infoMenu.SetActive(false);
        gm.ResumeGame();
    }

    void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    // Pause if not paused or resume if paused
    public void PauseOrResume()
    {
        if (gm.isPaused)
        {
            gm.ResumeGame();
            HidePauseMenu();
        }
        else
        {
            gm.PauseGame();
            ShowPauseMenu();
        }
    }

    // Skip the info message at the beginning of a level.
    public void Skip()
    {
        infoMenu.SetActive(false);

        if (gm.isPaused)
        {
            gm.ResumeGame();
        }
    }
}
