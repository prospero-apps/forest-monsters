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
                gm.SaveData();
                SceneManager.LoadScene(gm.levelToLoad);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            doorText.gameObject.SetActive(false);
        }
    }
       
    // Open the door
    public void Open()
    {
        anim.enabled = true;
        player.PlaySound(doorClip);
        anim.Play("Door_Opening");
        isOpen = true;
    }

    // Close the door
    public void Close()
    {
        player.PlaySound(doorClip);
        anim.Play("Door_Closing");
        isOpen = false;
    }
}
