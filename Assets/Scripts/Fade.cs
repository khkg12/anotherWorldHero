using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float fadeTime;
    public SpriteRenderer fadeSprite;
    
    private void OnEnable()
    {
        fadeTime = 1.8f;
    }
    void Update()
    {
        if(fadeTime > 0f)
        {
            fadeSprite.color = new Color(0, 0, 0, fadeTime / 3);
            fadeTime -= Time.deltaTime;
        }
        else
        {            
            gameObject.SetActive(false);            
        }
        
    }
}