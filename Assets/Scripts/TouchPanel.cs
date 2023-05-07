using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerClickHandler
{    
    public void OnPointerClick(PointerEventData eventData)
    {
        DialogManager.Instance.ClickFlag = true;
        if (DialogManager.Instance.SkipFlag == true)
        {            
            DialogManager.Instance.DialogFlag = true;
        }
    }
}
