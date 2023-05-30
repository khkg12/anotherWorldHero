using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float fadeTime;
    public float fadeSpeed; 
    private float fadetime;
    
    public SpriteRenderer fadeSprite;
    public int R;
    public int G;
    public int B;
    
    private void OnEnable()
    {
        fadetime = fadeTime;
    }
    void Update()
    {
        if(fadetime > 0f)
        {
            fadeSprite.color = new Color(R, G, B, fadetime / fadeSpeed);
            fadetime -= Time.deltaTime;
        }
        else
        {            
            gameObject.SetActive(false);            
        }        
    }
}
