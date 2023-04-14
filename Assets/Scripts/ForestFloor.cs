using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestFloor : MonoBehaviour
{
    private PlayerController player;
    private Vector3 originalPlayerPosition;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // When the Player falls down to the forest floor, they die and the scene is reloaded.
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.Die();
        }
    }
}
