using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffect : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DisAppear", 1f);
    }

    public void DisAppear()
    {
        gameObject.SetActive(false);
    }
}
