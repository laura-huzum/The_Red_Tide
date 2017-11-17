using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipToggler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.Find("Image").gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.Find("Image").gameObject.SetActive(false);
    }
}
