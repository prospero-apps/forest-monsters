using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public bool fadeIn = false;
    public bool fadeOut = false;

    // How long it takes to fade.
    public float duration;
        
    void Update()
    {
        if (fadeIn)
        {
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += duration * Time.deltaTime;
                if (canvasGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }

        if (fadeOut)
        {
            if (canvasGroup.alpha >= 0)
            {
                canvasGroup.alpha -= duration * Time.deltaTime;
                if (canvasGroup.alpha <= 0)
                {
                    fadeOut = false;
                }
            }
        }
    }

    /// <summary>
    /// Make the scene fade in
    /// </summary>
    public void FadeIn()
    {
        fadeIn = true;
    }

    /// <summary>
    /// Make the scene fade out
    /// </summary>
    public void FadeOut()
    {
        fadeOut = true;
    }
}
