using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRange : MonoBehaviour
{
    // References
    private PlayerController player;
    [SerializeField] private Door door;
        
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
       
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {            
            if (player.hasKey && !door.isOpen)
            {
                door.Open();
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (door.isOpen)
            {
                door.Close();
            }
        }        
    }
}
