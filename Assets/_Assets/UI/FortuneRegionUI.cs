using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FortuneRegionUI : EventTrigger
{
    public Fortune fortune;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        FortuneDisplay.Instance.AddFortune(fortune);

        Debug.Log("Mouse inside object!!!");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        FortuneDisplay.Instance.RemoveFortune(fortune);

        Debug.Log("Mouse left object!!!");
    }
}
