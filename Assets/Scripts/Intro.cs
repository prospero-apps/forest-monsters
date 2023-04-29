using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    // References
    private GameManager gm;
    private AudioSource audioSource;

    // Volume
    private static float originalVolume;

    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioSource = gm.GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        gm.SaveVolume();
    }

    void Start()
    {
        StartCoroutine(FadeSound(audioSource, 3.0f, 0));
        StartCoroutine(GoToMainMenu());
    }    

    IEnumerator GoToMainMenu()
    {
        yield return new WaitForSeconds(6.0f);
        audioSource.volume = originalVolume;
        audioSource.mute = true;
        gm.SaveVolume();
        SceneManager.LoadScene("MainMenu");
    }

    public static IEnumerator FadeSound(AudioSource audioSource, float duration, float targetVolume)
    {
        originalVolume = audioSource.volume;

        yield return new WaitForSeconds(3.0f);

        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / duration);
            yield return null;
        }

        yield break;
    }
}
