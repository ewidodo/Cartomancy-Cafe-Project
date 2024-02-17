using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FortuneRegionUI : EventTrigger
{
    public Fortune fortune;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        // add fortune to fortune display "list"

        Debug.Log("Mouse inside object!!!");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        // remove fortune from fortune display "list"

        Debug.Log("Mouse left object!!!");
    }
}
