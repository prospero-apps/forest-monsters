using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GoToMainMenu());
    }    

    IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
