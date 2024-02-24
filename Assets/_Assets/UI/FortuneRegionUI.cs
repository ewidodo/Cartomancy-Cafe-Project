using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FortuneRegionUI : EventTrigger
{
    public Fortune fortune;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        FortuneDisplay.Instance.AddHoveredFortune(fortune);

        Debug.Log("Mouse inside object!!!");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        FortuneDisplay.Instance.RemoveHoveredFortune(fortune);

        Debug.Log("Mouse left object!!!");
    }
}
