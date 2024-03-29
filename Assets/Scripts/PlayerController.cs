using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // References
    private Rigidbody2D rb2d;
    private Animator anim;
    private GameManager gm;
    private AudioSource audioSource;

    // How fast does the player move in horizontal direction?
    [SerializeField] private float speed = 10;

    // Maximum speed
    [SerializeField] private float maxSpeed = 3;

    // How much can the Player jump?
    [SerializeField] private float jumpForce = 520;

    // Which direction is the Player looking in?
    private Vector2 lookDirection = new Vector2(1, 0);

    // Current and maximum health
    public int currentHealth;
    [SerializeField] private int maxHealth = 5;

    // Current and maximum ammo
    public int currentAmmo;
    [SerializeField] private int maxAmmo;

    // Is the Player invisible?
    public bool isInvisible = false;    

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
    [SerializeField] private bool isPoweredUp = false;

    // Is the Player protected by a shield?
    public bool isProtectedByShield = false;

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

    // Audio clips
    [SerializeField] private AudioClip collectClip;
    [SerializeField] private AudioClip healthClip;
    [SerializeField] private AudioClip hitPlayerClip;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip shootPoweredClip;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioSource = gameObject.GetComponent<AudioSource>();

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
            if (!gm.isPaused && currentAmmo > 0)
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

        // Die if there's no more health left
        if (currentHealth <= 0)
        {
            Die();
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
            PlaySound(collectClip);

            // Their ammo is full again.
            currentAmmo = maxAmmo;

            Destroy(col.gameObject);
        }

        // If they bump into a crystal ball...
        if (col.CompareTag("Crystal Ball"))
        {
            PlaySound(collectClip);

            // Make the sorcerer visible.
            GameObject.FindGameObjectWithTag("Sorcerer").GetComponent<SorcererController>().MakeVisible();

            Destroy(col.gameObject);
        }

        // If they bump into a first aid kit...
        if (col.CompareTag("First Aid Kit"))
        {
            PlaySound(healthClip);

            // Their health is full again.
            currentHealth = maxHealth;

            Destroy(col.gameObject);
        }

        // If they bump into an invisibility cloak...
        if (col.CompareTag("Invisibility Cloak"))
        {
            PlaySound(collectClip);

            // They become invisible.
            isInvisible = true;

            Destroy(col.gameObject);
        }

        // If they bump into poison...
        if (col.CompareTag("Poison"))
        {
            PlaySound(collectClip);

            // They are slowed down.
            isSlowedDown = true;

            Destroy(col.gameObject);
        }

        // If they bump into a power drink...
        if (col.CompareTag("Power Drink"))
        {
            PlaySound(collectClip);

            // They are powered up
            isPoweredUp = true;

            Destroy(col.gameObject);
        }

        // If they bump into a shield...
        if (col.CompareTag("Shield"))
        {
            PlaySound(collectClip);

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
            PlaySound(collectClip);

            // They collect the key and become its parent. The key is attached to the Player.
            hasKey = true;

            col.gameObject.transform.parent = transform;
            Vector3 offsetPosition = transform.position + new Vector3(0, 0.8f, 0);
            col.gameObject.transform.position = offsetPosition;
        }

        // If they get shot...
        if (col.CompareTag("MonsterMissile") || col.CompareTag("SorcererMissile"))
        {
            Destroy(col.gameObject);

            PlaySound(hitPlayerClip);

            if (!isProtectedByShield)
            {
                currentHealth--;
                // The monster should flash briefly to signal it's been hit.
                anim.Play("Player_Flashing");
            }             
        }
    }

    /// <summary>
    /// Make the Player die and reload the scene. If this is the last level,
    /// the Player will lose the game if they've used up all chances.
    /// </summary>
    public void Die()
    {
        // If this is Level 10...
        if (SceneManager.GetActiveScene().name == "Level10")
        {
            // If the Player has any chances left...
            if (gm.chances > 1)
            {
                // One chance is lost...
                gm.LoseChance();

                // And the level starts again.
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            // Otherwise the game is lost.
            else
            {
                SceneManager.LoadScene("GameOverFailure");
            }
        }
        // In any other level just start the level again.
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }        
    }

    /// <summary>
    /// Remove the shield when the time the Player is protected by it is up
    /// </summary>
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

    /// <summary>
    /// Make the Player take damage if they bump into a monster or get shot
    /// </summary>
    /// <param name="damageAmount">Amount of damage</param>
    /// <param name="direction">Knockback direction</param>
    /// <param name="knockback">Whether there should be knockback</param>
    /// <param name="knockbackPowerX">Amount of knockback on horizotal axis</param>
    /// <param name="knockbackPowerY">Amount of knockback on vertical axis</param>
    public void Damage(int damageAmount, float direction, bool knockback = false, float knockbackPowerX = 1000, float knockbackPowerY = 50)
    {        
        currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0, maxHealth);

        // The Player should flash briefly to show us something bad has happened.
        anim.Play("Player_Flashing");

        PlaySound(hitPlayerClip);

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

    /// <summary>
    /// Make the Player shoot in the direction they're facing and reduce their ammo by one
    /// </summary>
    public void Shoot()
    {
        GameObject bulletInstance;

        if (isPoweredUp)
        {
            bulletInstance = Instantiate(powerBullet, shootPoint.transform.position, Quaternion.identity);
            PlaySound(shootPoweredClip);
        }
        else
        {
            bulletInstance = Instantiate(bullet, shootPoint.transform.position, Quaternion.identity);
            PlaySound(shootClip);
        }

        Missile missile = bulletInstance.GetComponent<Missile>();
        missile.Launch(lookDirection);

        currentAmmo--;
    }

    /// <summary>
    /// Play a sound
    /// </summary>
    /// <param name="clip">Clip to play</param>
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Make the Player stop
    /// </summary>
    public void StopMoving()
    {
        speed = 0;
    }
}
