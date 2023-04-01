using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyCanvas : MonoBehaviour
{    
    void Awake()
    {
        var thisObj = FindObjectsOfType<DontDestroyCanvas>();  // FindObjectsOfType : 해당하는 타입의 오브젝트를 찾아서 배열로줌
        if (thisObj.Length == 1)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
