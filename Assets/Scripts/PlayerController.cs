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

    // How much can the Player jump?
    [SerializeField] private float jumpForce = 520;

    // Which direction is the Player looking in?
    private Vector2 lookDirection = new Vector2(1, 0);

    // Current and maximum health
    private int currentHealth;
    [SerializeField] private int maxHealth;

    // Current and maximum ammo
    private int currentAmmo;
    [SerializeField] private int maxAmmo;

    // Is the Player invisible?
    [SerializeField] bool isInvisible = false;

    // References
    private Rigidbody2D rb2d;
    private Animator anim;

    // Shooting
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject powerBullet;
    [SerializeField] private Transform shootPoint;

    // Movement
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
    public bool hasKey = false;

    // TIMERS
    private float powerupTimer;
    // How long will the Player remain powered up?
    [SerializeField] private float powerupTime = 30;       

    private float invisibleTimer;
    // How long will the Player remain invisible?
    [SerializeField] private float invisibleTime = 30;     

    private float shieldTimer;
    // How long will the Player be protected from bullets?
    [SerializeField] private float shieldTime = 30;     

    private float slowdownTimer;
    // How long will the Player be slowed down after drinking poison?
    [SerializeField] private float slowdownTime = 15;     


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Let's start with full health and ammo.
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
    }
        
    void Update()
    {
        // Set the animator parameters
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        anim.SetBool("Grounded", isGrounded);
        anim.SetBool("Invisible", isInvisible);

        horizontal = Input.GetAxis("Horizontal");

        // Make the player always look in movement direction
        if (horizontal < -0.1f)
        {
            // look left
            transform.localScale = new Vector3(-1, 1, 1);
            lookDirection.Set(-1, 0);
        }

        if (horizontal > 0.1f)
        {
            // look right
            transform.localScale = new Vector3(1, 1, 1);
            lookDirection.Set(1, 0);
        }

        lookDirection.Normalize();

        // Check if the player is jumping
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        // Shoot 
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
        }

        // Handle the timers
        if (powerupTimer > powerupTime)
        {
            isPoweredUp = false;
        }

        if (isPoweredUp)
        {
            powerupTimer += Time.deltaTime;
        }

        if (invisibleTimer > invisibleTime)
        {
            isInvisible = false;
        }

        if (isInvisible)
        {
            invisibleTimer += Time.deltaTime;
        }

        if (shieldTimer > shieldTime)
        {
            isProtectedByShield = false;
            RemoveShield();
        }

        if (isProtectedByShield)
        {
            shieldTimer += Time.deltaTime;
        }

        if (slowdownTimer > slowdownTime)
        {
            isSlowedDown = false;
        }

        if (isSlowedDown)
        {
            slowdownTimer += Time.deltaTime;
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

        // Move at half the speed if Player is slowed down or at full speed otherwise
        if (isSlowedDown)
        {
            rb2d.AddForce(Vector2.right * speed / 2 * horizontal);
        }
        else
        {
            rb2d.AddForce(Vector2.right * speed * horizontal);
        }
        
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

        // If they get shot...
        if (col.CompareTag("MonsterMissile") || col.CompareTag("SorcererMissile"))
        {

        }
    }

    // The player dies and the scene is reloaded.
    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Remove the shield when the time the Player is protected by it is up
    private void RemoveShield()
    {
        // If the shield is a child of the Player...
        if (gameObject.transform.Find("Shield") != null)
        {
            // Find it and remove it.
            GameObject shield = GameObject.FindGameObjectWithTag("Shield");
            Destroy(shield);
            isProtectedByShield = false;
        }
    }

    // The Player takes damage if they bump into a monster or get shot.
    public void Damage(int damageAmount, float direction, bool knockback = false, float knockbackPowerX = 1000, float knockbackPowerY = 50)
    {
        currentHealth -= damageAmount;

        // The Player should flash briefly to show us something bad has happened.
        anim.Play("Player_Flashing");

        if (knockback)
        {
            StartCoroutine(Knockback(0.03f, knockbackPowerX, knockbackPowerY, direction));
        }
    }
      
    // If the Player bumps into a monster or a bomb, they knock back for a very short period of time.
    public IEnumerator Knockback(float knockDuration, float knockbackPowerX, float knockbackPowerY, float knockbackDirection)
    {
        float timer = 0;

        while (knockDuration > timer)
        {
            timer += Time.deltaTime;

            rb2d.AddForce(new Vector3(knockbackDirection * knockbackPowerX, transform.position.y + knockbackPowerY,
                transform.position.z));
        }

        yield return 0;
    }

    // Shoot
    public void Shoot()
    {
        GameObject bulletInstance = isPoweredUp ?
            Instantiate(powerBullet, shootPoint.transform.position, Quaternion.identity) :
            Instantiate(bullet, shootPoint.transform.position, Quaternion.identity);

        Missile missile = bulletInstance.GetComponent<Missile>();
        missile.Launch(lookDirection);

        currentAmmo--;
    }
}
