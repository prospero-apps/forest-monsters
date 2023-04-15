using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // How fast does the player move in horizontal direction?
    [SerializeField] private float speed = 10;

    // Maximum speed
    [SerializeField] private float maxSpeed = 3;

    // How much can the player jump?
    [SerializeField] private float jumpForce = 520;

    // Current and maximum health
    private int currentHealth;
    [SerializeField] private int maxHealth;

    // Current and maximum ammo
    private int currentAmmo;
    [SerializeField] private int maxAmmo;

    // Is the Player invisible?
    [SerializeField] bool isInvisible = false;

    private Rigidbody2D rb2d;
    private float horizontal;
    private bool isJumping;

    // Is the player standing on a platform?
    public bool isGrounded;

    // Can the player double-jump?
    private bool canDoubleJump;

    // How many times the second part of a double jump is to be weaker than the first
    private float doubleJumpReducer = 1.75f;

    // Is the Player slowed down after drinking poison or powered up after drinking a power drink?
    private bool isSlowedDown = false;
    private bool isPoweredUp = false;

    // Is the Player protected by a shield?
    private bool isProtectedByShield = false;

    // Has the Player found the key?
    private bool hasKey = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        // Let's start with full health and ammo.
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
    }
        
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        // Make the player always look in movement direction
        if (horizontal < -0.1f)
        {
            // look left
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (horizontal > 0.1f)
        {
            // look right
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Check if the player is jumping
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }        
    }

    void FixedUpdate()
    {
        // Move horizontally
        if (isGrounded)
        {
            // Fake friction - ease the x speed
            Vector2 easeVelocity = rb2d.velocity;
            easeVelocity.x *= 0.95f;
            easeVelocity.y = rb2d.velocity.y;
            rb2d.velocity = easeVelocity;
        }

        rb2d.AddForce(Vector2.right * speed * horizontal);

        // Speed limit
        if (rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }

        if (rb2d.velocity.x < -maxSpeed)
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }

        // Jump
        if (isJumping)
        {
            if (isGrounded)
            {
                rb2d.AddForce(Vector2.up * jumpForce);
                canDoubleJump = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    canDoubleJump = false;
                    rb2d.AddForce(Vector2.up * jumpForce / doubleJumpReducer);
                }
            }
            
            isJumping = false;
        }
    }

    // The player dies and the scene is reloaded.
    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // What happens if the Player bumps into something?
    void OnTriggerEnter2D(Collider2D col)
    {
        // If they bump into ammo...
        if (col.CompareTag("Ammo"))
        {
            // Their ammo is full again.
            currentAmmo = maxAmmo;

            Destroy(col.gameObject);
        }

        // If they bump into a crystal ball...
        if (col.CompareTag("Crystal Ball"))
        {
            // TODO: They will be able to see the sorcerer.
            
            Destroy(col.gameObject);
        }

        // If they bump into a first aid kit...
        if (col.CompareTag("First Aid Kit"))
        {
            // Their health is full again.
            currentHealth = maxHealth;

            Destroy(col.gameObject);
        }

        // If they bump into an invisibility cloak...
        if (col.CompareTag("Invisibility Cloak"))
        {
            // They become invisible.
            isInvisible = true;

            Destroy(col.gameObject);
        }

        // If they bump into poison...
        if (col.CompareTag("Poison"))
        {
            // They are slowed down.
            isSlowedDown = true;

            Destroy(col.gameObject);            
        }

        // If they bump into a power drink...
        if (col.CompareTag("Power Drink"))
        {
            // They are powered up
            isPoweredUp = true;

            Destroy(col.gameObject);            
        }

        // If they bump into a shield...
        if (col.CompareTag("Shield"))
        {
            // They are protected by the shield.
            isProtectedByShield = true;

            // The shield becomes a child of the Player and is attached to them.
            col.gameObject.transform.parent = transform;
            Vector3 offsetPosition = transform.position + new Vector3(0, 0.8f, 0);
            col.gameObject.transform.position = offsetPosition;
        }

        // If they bump into a key...
        if (col.CompareTag("Key"))
        {
            // They collect the key and become its parent. The key is attached to the Player.
            hasKey = true;

            col.gameObject.transform.parent = transform;
            Vector3 offsetPosition = transform.position + new Vector3(0, 0.8f, 0);
            col.gameObject.transform.position = offsetPosition;            
        }
    }
}
