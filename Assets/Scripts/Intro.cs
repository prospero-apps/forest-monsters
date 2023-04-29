using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    // References
    private GameManager gm;
    private AudioSource audioSource;
    private Fade fade;

    // Volume
    private static float originalVolume;

    void Awake()
    {
        // Start the game with a set volume and save its value
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioSource = gm.GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
        gm.SaveVolume();
    }

    void Start()
    {
        fade = FindObjectOfType<Fade>();

        // The sound should fade out and the scene should transition to Main Menu.
        StartCoroutine(FadeSound(audioSource, 3.0f, 0));
        StartCoroutine(GoToMainMenu());
    }    

    IEnumerator GoToMainMenu()
    {   
        // Display the intro for 6 seconds.
        yield return new WaitForSeconds(6.0f);

        // Handle the audio
        audioSource.volume = originalVolume;
        audioSource.mute = true;
        gm.SaveVolume();

        // Fade in
        fade.FadeIn();

        // Wait for another second and then change scene.
        yield return new WaitForSeconds(1.0f);
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
