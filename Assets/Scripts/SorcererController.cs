using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SorcererController : MonoBehaviour
{
    // References
    private PlayerController player;
    private SpriteRenderer spriteRenderer;

    // Shooting
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootPoint;

    // Here are the platforms on which the sorcerer may spawn (on a different random one each time).
    [SerializeField] private Platform[] platforms;
        
    // Which direction is the sorcerer facing?
    [SerializeField] private float direction;
        
    // Distance to the Player
    private float distanceToPlayer;
            
    // The sorcerer's current and maximum health
    private int currentHealth;

    void Start()
    {
        // Let's get reference to the Player.
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // Let's get reference to the SpriteRenderer.
        spriteRenderer = GetComponent<SpriteRenderer>();
                
        // Spawn on a random platform. 
        Spawn();

        // Calculate the sorcerer's health.
        CalculateHealth();

        // The sorcerer should start invisible, but still dangerous.
        //MakeInvisible();
    }


    void Update()
    {
        // Look right or left
        if (direction < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (direction > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Die if there's no more health left. 
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        // The sorcerer should look in the direction of the Player in order to shoot at them.
        if (player.gameObject.transform.position.x >= transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }

    // Spawn on a random platform.
    void Spawn()
    {
        // Choose the platform to spawn on and become its child.
        Platform randomPlatform = platforms[Random.Range(0, 9)];
        transform.parent = randomPlatform.transform;

        // Position the sorcerer in the middle of the platform.
        transform.position = new Vector3(randomPlatform.transform.position.x, randomPlatform.transform.position.y + 0.25f, 0);
    }
        
    void CalculateHealth()
    {
        // The sorcerer's health depends on the Player's score. Temporarily it's constant.
        currentHealth = 20;
    }
      
    void MakeInvisible()
    {
        spriteRenderer.enabled = false;
    }

    void MakeVisible()
    {
        spriteRenderer.enabled = true;
    }

    void Die()
    {
        // Remove Sorcerer
        Destroy(gameObject);
    }    
}
