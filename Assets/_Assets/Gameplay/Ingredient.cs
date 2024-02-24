using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// The data component of anything that will affect the final fortune
public class Ingredient : MonoBehaviour
{
    [Header("Ingredient Information")]
    public string name;
    public string description;
    public Vector2 fortuneOffset = new();
    [HideInInspector] public bool inDrink = false;

    [Header("Display References")]
    public GameObject card;
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI descriptionDisplay;
    public TextMeshProUGUI fortuneOffsetDisplay;

    [Header("Display Parameters")]
    [SerializeField] private float hoverScaleMultiplier;
    [SerializeField] private float hoverScaleTime;
    public Vector3 defaultScale = Vector3.one;


    private void Awake()
    {
        InitDisplay();
    }

    private void InitDisplay()
    {
        nameDisplay.text = name;
        descriptionDisplay.text = description;
        fortuneOffsetDisplay.text = fortuneOffset.ToString();
    }

    public void Enlarge()
    {
        // Display in front
        GetComponent<Canvas>().sortingOrder = 1;
        // Cancel any other ongoing scale tweens
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject,
                        callOnUpdate: (color) => { card.transform.localScale = new Vector3(color.r, color.g, color.b); },
                        new Color(defaultScale.x, defaultScale.y, defaultScale.z, 1),
                        new Color(hoverScaleMultiplier, hoverScaleMultiplier, hoverScaleMultiplier),
                        hoverScaleTime);
    }

    public void ResetSize()
    {
        // Display normally
        GetComponent<Canvas>().sortingOrder = 0;
        // Cancel any other ongoing scale tweens
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject, 
                        callOnUpdate: (color) => { card.transform.localScale = new Vector3(color.r, color.g, color.b); }, 
                        new Color(card.transform.localScale.x, card.transform.localScale.y, card.transform.localScale.z),
                        new Color(defaultScale.x, defaultScale.y, defaultScale.z, 1),
                        hoverScaleTime);
    }

    public void SelectIngredient()
    {
        if (!inDrink)
        {
            Barista.Instance?.UseIngredient(this);
        }
        else if (inDrink)
        {
            Barista.Instance.RemoveIngredient(this);
        }
    }
}
