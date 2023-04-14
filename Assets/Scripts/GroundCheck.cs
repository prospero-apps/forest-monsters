using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private PlayerController player;
        
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    // Check if the player is standing on a platform.
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Platform"))
        {
            player.isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Platform"))
        {
            player.isGrounded = false;
        }
    }
}
