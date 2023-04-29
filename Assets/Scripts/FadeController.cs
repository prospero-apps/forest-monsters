using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private Fade fade;
        
    void Start()
    {
        fade = FindObjectOfType<Fade>();   
        fade.FadeOut();
    }
}
