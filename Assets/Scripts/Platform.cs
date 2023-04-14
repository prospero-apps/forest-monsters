using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Each platform is permanently immobile, permanently mobile or becomes
/// mobile only when a character jumps onto it.
/// </summary>
public enum Mobility
{
    Immobile,
    Mobile,
    MobileOnEnter
}

public class Platform : MonoBehaviour
{
    // Is the platform mobile? Immobile? Mobile only when entered?
    [SerializeField] private Mobility mobility = Mobility.Immobile;

    // How fast does the platform move from side to side if mobile?
    [SerializeField] private float speed = 3;

    // Which direction is the platform moving in? Left or right?
    [SerializeField] private float direction = 1;

    // How long does it take for a platform to start moving on player enter?
    [SerializeField] private float mobilityDelay = 1;

    // Min and max x positions for the mobile platform
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    // Whether the platform was originally mobile on enter
    private bool kicked = false;

    private Rigidbody2D rb2d;
        
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (mobility == Mobility.Mobile)
        {
            // move right, then left, etc.
            if (transform.position.x > maxX && direction > 0.0f) 
            {
                direction *= -1;
            }
            if (transform.position.x < minX && direction < 0.0f) 
            {
                direction *= -1;
            }

            transform.Translate(speed * Time.deltaTime * direction, 0, 0);
        }
    }

    // If the Player lands on a platform...
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            // They become the platform's child.
            col.gameObject.transform.parent = transform;

            // If the platform should start moving, let's make sure the Player hit it from above.
            if (mobility == Mobility.MobileOnEnter && col.gameObject.transform.position.y > transform.position.y)
            {
                // If so, kick the platform so that it starts moving.
                StartCoroutine(Kick(true));
            }
        }
    }

    // If the Player leaves the platform...
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            if (col.gameObject.transform.IsChildOf(transform))
            {
                // The Player should no longer be the platform's child.
                col.gameObject.transform.parent = null;
            }

            // If the Player leaves a MobileOnEnter platform, which is now mobile and kicked...
            if (mobility == Mobility.Mobile && kicked)
            {
                // The platform should stop moving.
                StartCoroutine(Kick(false));
            }
        }
    }

    // The couroutine to handle the platform's movement
    IEnumerator Kick(bool intoMobile)
    {
        // The platform was kicked, so let's make it mobile.
        if (intoMobile)
        {
            // The platform should start moving after a while, not right away.
            yield return new WaitForSeconds(mobilityDelay);
            
            mobility = Mobility.Mobile;
            kicked = true;
        }
        else
        {
            // The Player left the platform, so let it stop right away.
            mobility = Mobility.MobileOnEnter;
            kicked = false;
        }
    }
}
