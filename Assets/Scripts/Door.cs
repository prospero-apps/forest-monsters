using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
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
