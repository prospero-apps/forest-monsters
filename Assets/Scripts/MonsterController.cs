using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    // References
    private PlayerController player;
    private Rigidbody2D rb2d;
    private Animator anim;

    // Shooting
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootPoint;

    // How often should a monster shoot?
    [SerializeField] private float shootInterval;

    // Min distance from player at which the monster starts shooting
    [SerializeField] private float shootDistance;

    // How fast does the monster move in horizontal direction?
    [SerializeField] private float speed = 2;

    // Which direction is the monster facing?
    [SerializeField] private float direction;

    // How far from their original position can the monster walk?
    [SerializeField] private float walkingDistance = 1;

    // Distance to the Player
    private float distanceToPlayer;

    // Is the monster mobile?
    [SerializeField] private bool isMobile;

    // Can the monster shoot?
    [SerializeField] private bool isShooter;

    // The monster's original position
    private Vector3 originalPosition;

    // The monster's current and maximum health
    private int currentHealth;
    [SerializeField] private int maxHealth;

    // Bullet timer
    private float bulletTimer;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Let's get reference to the Player.
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // Let's remember the monster's original position.
        originalPosition = transform.position;

        // The monster starts with full health.
        currentHealth = maxHealth;

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

        // Die if there's no more health left
        if (currentHealth <= 0)
        {
            Die();
        }

        // Check whether you can shoot and do so if possible
        if (isShooter && !player.isInvisible)
        {
            // Check if the Player is within shooting distance 
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= shootDistance)
            {
                // Update the bullet timer and check whether the shoot interval is over
                bulletTimer += Time.deltaTime;

                if (bulletTimer >= shootInterval)
                {
                    Shoot();
                }
            }
        }        
    }

    void FixedUpdate()
    {
        // If the monster is mobile...
        if (isMobile)
        {
            // Fake friction - ease the x speed
            Vector2 easeVelocity = rb2d.velocity;
            easeVelocity.x *= 0.95f;
            easeVelocity.y = rb2d.velocity.y;
            rb2d.velocity = easeVelocity;

            // They keep moving back and forth...
            rb2d.AddForce(Vector2.right * speed * direction);

            // turning left toward their original position
            if (transform.position.x >= originalPosition.x + walkingDistance && direction > 0.0f) 
            {
                direction *= -1;
            }
            // and back right
            if (transform.position.x <= originalPosition.x && direction < 0.0f) 
            {
                direction *= -1;
            }

        }
        // If the monster is immobile...
        else
        {
            // We just set the direction depending on where the player is so that the monster shoots in the right direction.
            if (player.gameObject.transform.position.x >= transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // If a moster collides with the Player, the latter should take damage.
        if (col.CompareTag("Player"))
        {
            // Find direction to Player (1 or -1 depending on where the Player is relative to the monster). 
            float directionToPlayer = Mathf.Sign(player.transform.position.x - transform.position.x);

            player.Damage(1, directionToPlayer, true);
        }

        // If a monster gets shot...
        if (col.CompareTag("PlayerMissile"))
        {
            Destroy(col.gameObject);
            currentHealth--;

            // The monster should flash briefly to signal it's been hit.
            anim.Play("Flash");
        }

        if (col.CompareTag("PlayerPowerMissile"))
        {
            Destroy(col.gameObject);
            currentHealth -= 4;

            // The monster should flash briefly to signal it's been hit.
            anim.Play("Flash");
        }
    }

    void Die()
    {
        // Remove Monster
        Destroy(gameObject);
    }

    void Shoot()
    {
        Vector2 shootDirection = new Vector2(direction, 0);
        shootDirection.Normalize();

        GameObject bulletInstance = Instantiate(bullet, shootPoint.transform.position, Quaternion.identity);

        Missile missile = bulletInstance.GetComponent<Missile>();
        missile.Launch(shootDirection);

        bulletTimer = 0;
    }
}
