using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // How far away is the player from the door?
    private float distanceToPlayer;

    // Range within which the player will make the door open.
    [SerializeField] private float wakeRange = 3;

    // Is the door open?
    private bool isOpen = false;

    // References
    private PlayerController player;
    private Animator anim;
    private GameManager gm;

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

    void Update()
    {
        // Check whether the Player has found the key.
        if (player.hasKey)
        {
            // Check whether the Player is close enough for the door to open.
            RangeCheck();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        { 
            // Inform the Player they need the key.
            if (!player.hasKey)
            {
                gm.doorText.gameObject.SetActive(true);
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
            gm.doorText.gameObject.SetActive(false);
        }
    }

    void RangeCheck()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Is the door close enough?
        if (distanceToPlayer <= wakeRange)
        {
            anim.enabled = true;
            anim.Play("Door_Opening");
            isOpen = true;
        }
        else
        {
            // If the door is open, close it.
            if (isOpen)
            {
                anim.Play("Door_Closing");
                isOpen = false;
            }            
        }
    }    
}
