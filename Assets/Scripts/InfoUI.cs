using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoUI : MonoBehaviour
{
    // References
    private GameManager gm;

    // The UI GameObject
    [SerializeField] private GameObject Info;
    
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        StartCoroutine(HideInfo());
    }

    // The level info should disappear after a while
    IEnumerator HideInfo()
    {
        gm.PauseGame();
        yield return new WaitForSecondsRealtime(10);
        Info.SetActive(false);     
        gm.ResumeGame();
    }
}
