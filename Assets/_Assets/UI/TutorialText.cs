using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    private TMP_Text text;

    [Header("Colors")]
    private Color defaultColor;
    public Color highlightColor;
    public float flashTime;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        defaultColor = text.color;
    }

    private void OnEnable()
    {
        AnimateTextHighlight();
    }

    private void AnimateTextHighlight()
    {
        LeanTween.value(this.gameObject,(color) => { text.color = color; }, defaultColor,  highlightColor, flashTime).setOnComplete(AnimateTextDefault);
    }

    private void AnimateTextDefault()
    {
        LeanTween.value(this.gameObject, (color) => { text.color = color; }, highlightColor, defaultColor, flashTime).setOnComplete(AnimateTextHighlight);

    }
}
