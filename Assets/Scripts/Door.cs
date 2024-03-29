using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Door : MonoBehaviour
{
    // Is the door open?
    public bool isOpen = false;

    // References
    private PlayerController player;
    private Animator anim;
    private GameManager gm;
    private Fade fade;

    // UI
    public TMP_Text doorText;

    // Audio clip
    [SerializeField] private AudioClip doorClip;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        fade = FindObjectOfType<Fade>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        anim.enabled = false;
    }       

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // Inform the Player they need the key.
            if (!player.hasKey)
            {
                doorText.gameObject.SetActive(true);
            }
            // Save the data and let the Player pass to the next level.
            else if (isOpen)
            {
                player.StopMoving();
                gm.SaveData();
                StartCoroutine(NextLevel());
            }
        }
    }

    IEnumerator NextLevel()
    {        
        // Fade in
        fade.FadeIn();

        // Wait for a second and then change scene.
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(gm.levelToLoad);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            doorText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Open the door
    /// </summary>
    public void Open()
    {
        anim.enabled = true;
        player.PlaySound(doorClip);
        anim.Play("Door_Opening");
        isOpen = true;
    }

    /// <summary>
    /// Close the door
    /// </summary>
    public void Close()
    {
        player.PlaySound(doorClip);
        anim.Play("Door_Closing");
        isOpen = false;
    }
}
