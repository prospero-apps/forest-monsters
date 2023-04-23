using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // References
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject info;
    private GameManager gm;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    
    void Update()
    {
        // Pause or resume if Escape is pressed provided the Info UI is not visible
        //if (Input.GetButtonDown("PauseResume") && !info.activeInHierarchy)
        if (Input.GetButtonDown("Cancel"))
        {
            //gm.PauseOrResume();
            Debug.Log("aaa");
            info.SetActive(true);
        }

        if (gm.isPaused)
        {
            ShowPauseMenu();
        }
        else
        {
            HidePauseMenu();
        }
    }

    void ShowPauseMenu()
    {
        pause.SetActive(true);
    }

    void HidePauseMenu()
    {
        pause.SetActive(false);
    }
}
