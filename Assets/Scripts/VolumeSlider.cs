using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    // References
    [SerializeField] private Slider slider;

    // Volume key
    private static string volumeKey = "VOLUME";

    void Start()
    {   
        if (PlayerPrefs.HasKey(volumeKey))
        {
            slider.value = PlayerPrefs.GetFloat(volumeKey);
        }
    }
}
