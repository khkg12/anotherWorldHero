using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyCanvas : MonoBehaviour
{    
    void Awake()
    {
        var thisObj = FindObjectsOfType<DontDestroyCanvas>();  // FindObjectsOfType : �ش��ϴ� Ÿ���� ������Ʈ�� ã�Ƽ� �迭����
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
