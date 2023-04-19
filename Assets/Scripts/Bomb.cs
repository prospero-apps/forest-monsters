using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // References
    private PlayerController player;
    private Animator anim;
        
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
            

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        // Wake up and explode.
        anim.SetBool("Awake", true);

        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);

        // TODO: Harm the Player
    }
}
