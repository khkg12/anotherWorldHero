using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{    
    public RectTransform pos;    
    public TextMeshProUGUI myText;    
    float DisappearTime;
    Vector3 StartPos;
    [SerializeField]
    Vector3 NowPos;

    private void Start()
    {
        StartPos = pos.anchoredPosition;
    }
    private void OnEnable()
    {        
        DisappearTime = 1;
        NowPos += new Vector3(Random.Range(-5,5), Random.Range(-5,5));
        pos.localScale = new Vector2(1, 1);
        pos.anchoredPosition = NowPos;
        Invoke("Disappear", 1f);
    }

    void Update()
    {                
        pos.localScale -= new Vector3(0.1f, 0.1f) * 1f * Time.deltaTime;        
        myText.color = new Color(1, 0, 0, DisappearTime / 1);                
        DisappearTime -= Time.deltaTime;
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
        pos.anchoredPosition = StartPos;
        NowPos = StartPos;
    }
}
