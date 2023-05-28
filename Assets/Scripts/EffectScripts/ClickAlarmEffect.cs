using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickAlarmEffect : MonoBehaviour
{
    [SerializeField]
    private Image checkAlarm;
    float DisappearTime;

    private void OnEnable()
    {
        DisappearTime = 1.5f;
    }
    void Update()
    {
        if (DisappearTime < 0) { DisappearTime = 1.5f; }
        checkAlarm.color = new Color(checkAlarm.color.r, checkAlarm.color.g, checkAlarm.color.b, DisappearTime / 1.5f);
        DisappearTime -= Time.deltaTime; 
    }
}
