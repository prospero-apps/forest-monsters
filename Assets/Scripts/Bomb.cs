using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // References
    private PlayerController player;
    private Animator anim;

    // Audio clip
    [SerializeField] private AudioClip bombClip;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {        
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
                
        // Find direction to Player (1 or -1 depending on where the Player is relative to the bomb).
        float directionToPlayer = Mathf.Sign(player.transform.position.x - transform.position.x);

        // Harm the Player
        player.Damage(2, directionToPlayer, true, 600, 20);

        player.PlaySound(bombClip);
        Destroy(gameObject);
    }
}
