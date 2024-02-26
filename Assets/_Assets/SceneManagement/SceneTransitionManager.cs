using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneTransitionManager : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField, ReadOnly] private bool isTransitionActive = true;
    
    private enum TRANSITIONTYPE
    {
        FADE = 0,
        SLIDE = 1,
    }

    private enum SLIDEDIRECTION
    {
        RIGHT = 0,
        UP = 1,
        LEFT = 2,
        DOWN = 3,
    }

    [Header("Transition Settings")]
    [SerializeField] private TRANSITIONTYPE transitionType;
    [SerializeField] private SLIDEDIRECTION slideDirection;

    [HideInInspector] public UnityEvent transitionComplete;



    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        InitializeFadeFromBlack();
    }

    void InitializeFadeFromBlack()
    {
        _canvasGroup.alpha = 1;
    }

    public LTDescr TransitionFrom(float duration)
    {
        isTransitionActive = false;
        switch(transitionType)
        {
            case TRANSITIONTYPE.FADE:
                {
                    return FadeFromBlack(duration);
                }
            case TRANSITIONTYPE.SLIDE:
                {
                    return SlideFrom(duration);
                }
        }

        return null;
    }

    public LTDescr TransitionTo(float duration)
    {
        isTransitionActive = true;
        switch (transitionType)
        {
            case TRANSITIONTYPE.FADE:
                {
                    return FadeToBlack(duration);
                }
            case TRANSITIONTYPE.SLIDE:
                {
                    return SlideTo(duration);
                }
        }

        return null;
    }

    private LTDescr FadeToBlack(float duration)
    {
        return LeanTween.alphaCanvas(_canvasGroup, 1, duration);
    }

    private LTDescr FadeFromBlack(float duration)
    {
        return LeanTween.alphaCanvas(_canvasGroup, 0, duration);
    }

    private LTDescr SlideTo(float duration)
    {
        _canvasGroup.alpha = 1;

        switch (slideDirection)
        {
            case SLIDEDIRECTION.RIGHT:
                {
                    return LeanTween.value(this.gameObject,
                                           (Color color) => { _canvasGroup.transform.localPosition = new Vector3(color.r, color.g, color.b); },
                                           new Color(-SceneLoader.Instance.GetComponent<RectTransform>().sizeDelta.x, 0, 0),
                                           new Color(0, 0, 0),
                                           duration);
                }
            case SLIDEDIRECTION.UP:
                {
                    return LeanTween.value(this.gameObject,
                                           (Color color) => { _canvasGroup.transform.localPosition = new Vector3(color.r, color.g, color.b); },
                                           new Color(0, -SceneLoader.Instance.GetComponent<RectTransform>().sizeDelta.y, 0),
                                           new Color(0, 0, 0),
                                           duration);
                }
            case SLIDEDIRECTION.LEFT:
                {
                    return LeanTween.value(this.gameObject, 
                                           (Color color) => { _canvasGroup.transform.localPosition = new Vector3(color.r, color.g, color.b);  }, 
                                           new Color(SceneLoader.Instance.GetComponent<RectTransform>().sizeDelta.x, 0, 0), 
                                           new Color(0, 0, 0), 
                                           duration); 
                }
            case SLIDEDIRECTION.DOWN:
                {
                    return LeanTween.value(this.gameObject,
                                           (Color color) => { _canvasGroup.transform.localPosition = new Vector3(color.r, color.g, color.b); },
                                           new Color(0, SceneLoader.Instance.GetComponent<RectTransform>().sizeDelta.y, 0),
                                           new Color(0, 0, 0),
                                           duration);
                }
        }

        return null;
    }

    private LTDescr SlideFrom(float duration)
    {
        _canvasGroup.alpha = 1;

        switch (slideDirection)
        {
            case SLIDEDIRECTION.RIGHT:
                {
                    return LeanTween.value(this.gameObject,
                                           (Color color) => { _canvasGroup.transform.localPosition = new Vector3(color.r, color.g, color.b); },
                                           new Color(0, 0, 0),
                                           new Color(SceneLoader.Instance.GetComponent<RectTransform>().sizeDelta.x, 0, 0),
                                           duration).setOnComplete(() => { transitionComplete.Invoke(); });
                }
            case SLIDEDIRECTION.UP:
                {
                    return LeanTween.value(this.gameObject,
                                           (Color color) => { _canvasGroup.transform.localPosition = new Vector3(color.r, color.g, color.b); },
                                           new Color(0, 0, 0),
                                           new Color(0, SceneLoader.Instance.GetComponent<RectTransform>().sizeDelta.y, 0),
                                           duration);
                }
            case SLIDEDIRECTION.LEFT:
                {
                    return LeanTween.value(this.gameObject,
                                           (Color color) => { _canvasGroup.transform.localPosition = new Vector3(color.r, color.g, color.b); },
                                           new Color(0, 0, 0),
                                           new Color(-SceneLoader.Instance.GetComponent<RectTransform>().sizeDelta.x, 0, 0),
                                           duration);
                }
            case SLIDEDIRECTION.DOWN:
                {
                    return LeanTween.value(this.gameObject,
                                           (Color color) => { _canvasGroup.transform.localPosition = new Vector3(color.r, color.g, color.b); },
                                           new Color(0, 0, 0),
                                           new Color(0, -SceneLoader.Instance.GetComponent<RectTransform>().sizeDelta.y, 0),
                                           duration);
                }
        }

        return null;
    }

    public bool IsTransitionActive()
    {
        return isTransitionActive;
    }
}
