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
}
