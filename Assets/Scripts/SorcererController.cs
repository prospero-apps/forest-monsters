using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SorcererController : MonoBehaviour
{
    // References
    private PlayerController player;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private GameManager gm;
    private AudioSource audioSource;

    // Shooting
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootPoint;

    // How often should a monster shoot?
    private float shootInterval = 2;    

    // Here are the platforms on which the sorcerer may spawn (on a different random one each time).
    [SerializeField] private Platform[] platforms;
        
    // Which direction is the sorcerer facing?
    [SerializeField] private float direction;
        
    // Distance to the Player
    private float distanceToPlayer;

    // Min distance from player at which the Sorcerer starts shooting
    [SerializeField] private float shootDistance;

    // The sorcerer's current and maximum health
    private int currentHealth;

    // Bullet timer
    private float bulletTimer;

    // Audio clips
    [SerializeField] private AudioClip hitSorcererClip;
    [SerializeField] private AudioClip shootClip;

    void Start()
    {
        anim = GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioSource = gameObject.GetComponent<AudioSource>();

        // Let's get reference to the Player.
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // Let's get reference to the SpriteRenderer.
        spriteRenderer = GetComponent<SpriteRenderer>();
                
        // Spawn on a random platform. 
        Spawn();

        // Calculate the sorcerer's health.
        CalculateHealth();

        // The sorcerer should start invisible, but still dangerous.
        MakeInvisible();

        bulletTimer = 0;
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

        // Update the bullet timer and shoot if possible
        bulletTimer += Time.deltaTime;

        if (bulletTimer >= shootInterval)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer < shootDistance)
            {
                Shoot();
            }            
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

    void OnTriggerEnter2D(Collider2D col)
    {
        // If the Sorcerer collides with the Player, the latter should take damage.
        if (col.CompareTag("Player"))
        {
            // Find direction to Player (1 or -1 depending on where the Player is relative to the monster). 
            float directionToPlayer = Mathf.Sign(player.transform.position.x - transform.position.x);

            player.Damage(4, directionToPlayer, true);
        }

        // If the Sorcerer gets shot...
        if (col.CompareTag("PlayerMissile"))
        {
            Destroy(col.gameObject);
            currentHealth--;

            // The sorcerer should flash briefly to signal it's been hit.
            anim.Play("Flash");
            PlaySound(hitSorcererClip);
        }

        if (col.CompareTag("PlayerPowerMissile"))
        {
            Destroy(col.gameObject);
            currentHealth -= 2;

            // The sorcerer should flash briefly to signal it's been hit.
            anim.Play("Flash");
            PlaySound(hitSorcererClip);
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
        // The sorcerer's health depends on the Player's score. It's between 10 and 20.
        currentHealth = Mathf.Clamp(20 - (gm.score / 100), 10, 20);
    }
      
    void MakeInvisible()
    {
        spriteRenderer.enabled = false;
    }

    public void MakeVisible()
    {
        spriteRenderer.enabled = true;
    }

    void Die()
    {
        // Remove Sorcerer
        Destroy(gameObject);

        // The game is won
        SceneManager.LoadScene("GameOverSuccess");
    }

    void Shoot()
    {
        Vector2 shootDirection = new Vector2(direction, 0);
        shootDirection.Normalize();

        GameObject bulletInstance = Instantiate(bullet, shootPoint.transform.position, Quaternion.identity);
        PlaySound(shootClip);

        Missile missile = bulletInstance.GetComponent<Missile>();
        missile.Launch(shootDirection);

        bulletTimer = 0;
    }

    // Play a sound
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
