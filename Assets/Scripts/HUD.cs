using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    // References
    private PlayerController player;
    private GameManager gm;

    [SerializeField] private Sprite[] healthSprites;
    [SerializeField] private Image healthUI;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    
    void Update()
    {
        // The Player's current health is an int between 0 and 5, so let's use it as the index.
        healthUI.sprite = healthSprites[player.currentHealth];

        // Update the Player's ammo and score in the UI
        ammoText.text = "Ammo: " + player.currentAmmo;
        scoreText.text = "Score: " + gm.score;

        if (gm.highScore > 0)
        {
            highScoreText.text = "High Score: " + gm.highScore;
        }
    }
}
