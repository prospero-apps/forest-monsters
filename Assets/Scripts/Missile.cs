using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // References
    private Rigidbody2D rb2d;

    // How fast does the missile move?
    [SerializeField] private float force;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    
    // If the bullet is far away, remove it.
    void Update()
    {
        if (transform.position.magnitude > 50)
        {
            Destroy(gameObject);
        }
    }    

    public void Launch(Vector2 direction)
    {
        rb2d.velocity = direction * force;
    }
}
