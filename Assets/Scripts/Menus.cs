using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    // References
    private GameManager gm;

    // Menus
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject infoMenu;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        StartCoroutine(HideInfoMenu());
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
        gm.PauseGame();
        yield return new WaitForSecondsRealtime(3);
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
}
