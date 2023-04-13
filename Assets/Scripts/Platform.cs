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
    [SerializeField] private Mobility _mobility = Mobility.Immobile;

    // How fast does the platform move from side to side if mobile.
    [SerializeField] private float _speed = 3;

    // Which direction is the platform moving in? Left or right?
    [SerializeField] private float _direction = 1;

    // How long does it take for a platform to start moving on player enter?
    [SerializeField] private float _mobilityDelay = 1;

    // Min and max x positions for the mobile platform
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;

    private Rigidbody2D _rb2d;
        
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_mobility == Mobility.Mobile)
        {
            // move right, then left, etc.
            if (transform.position.x > _maxX && _direction > 0.0f) 
            {
                _direction *= -1;
            }
            if (transform.position.x < _minX && _direction < 0.0f) 
            {
                _direction *= -1;
            }

            transform.Translate(_speed * Time.deltaTime * _direction, 0, 0);
        }
    }
}