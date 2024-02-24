using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private bool isTransitionActive = true;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        InitializeFadeFromBlack();
    }

    void InitializeFadeFromBlack()
    {
        _canvasGroup.alpha = 1;
    }

    public LTDescr FadeFromBlack(float duration)
    {
        isTransitionActive = false;
        return LeanTween.alphaCanvas(_canvasGroup, 0, duration);
    }

    public LTDescr FadeToBlack(float duration)
    {
        isTransitionActive = true;
        return LeanTween.alphaCanvas(_canvasGroup, 1, duration);
    }

    public bool IsTransitionActive()
    {
        return isTransitionActive;
    }
}
