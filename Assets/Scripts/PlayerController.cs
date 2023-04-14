using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // How fast does the player move in horizontal direction?
    [SerializeField] private float speed = 10;

    // How much can the player jump?
    [SerializeField] private float jumpForce = 300;

    private Rigidbody2D rb2d;
    private float horizontal;
    private bool jumping;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
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
            jumping = true;
        }        
    }

    void FixedUpdate()
    {
        // Move horizontally
        rb2d.AddForce(Vector2.right * speed * horizontal);

        // Jump
        if (jumping)
        {
            rb2d.AddForce(Vector2.up * jumpForce);
            jumping = false;
        }
    }
}
