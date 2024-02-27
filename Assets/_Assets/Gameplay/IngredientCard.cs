using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// The data component of anything that will affect the final fortune
public class IngredientCard : MonoBehaviour
{
    [Header("Ingredient Information")]
    public Ingredient ingredient;
    [HideInInspector] public bool inDrink = false;

    [Header("Display References")]
    public GameObject card;
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI descriptionDisplay;
    public TextMeshProUGUI fortuneOffsetDisplay;
    public Image image;
    public TextMeshProUGUI number;
    [ReadOnly] public Arrow linkedArrow;
    [ReadOnly] public Arrow peekedArrow;

    [Header("Display Parameters")]
    [SerializeField] private float hoverScaleMultiplier;
    [SerializeField] private float hoverScaleTime;
    public Vector3 defaultScale = Vector3.one;
    public Color arrowHighlightColor;


    private void Awake()
    {
        InitDisplay();
    }

    private void OnDestroy()
    {
        //Destroy(linkedArrow.gameObject);
    }

    private void InitDisplay()
    {
        nameDisplay.text = ingredient.ingredientName;
        descriptionDisplay.text = ingredient.ingredientDescription;
        if (fortuneOffsetDisplay != null) fortuneOffsetDisplay.text = ingredient.fortuneOffset.ToString();
        image.sprite = ingredient.ingredientSprite;
        number.text = ingredient.ingredientNumber;
    }

    public void Enlarge()
    {
        // Display in front
        GetComponent<Canvas>().sortingOrder += 1;
        // Cancel any other ongoing scale tweens
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject,
                        callOnUpdate: (color) => { card.transform.localScale = new Vector3(color.r, color.g, color.b); },
                        new Color(defaultScale.x, defaultScale.y, defaultScale.z, 1),
                        new Color(hoverScaleMultiplier, hoverScaleMultiplier, hoverScaleMultiplier),
                        hoverScaleTime);

        // Highlight linked arrow on fortune display
        if (linkedArrow != null) linkedArrow.Highlight();
        else
        {
            // Display temp arrow at the end of the fortune table
            Barista.Instance.currentCustomer.PeekFortune(this);
        }
    }

    public void ResetSize()
    {
        // Display normally
        GetComponent<Canvas>().sortingOrder -= 1;
        // Cancel any other ongoing scale tweens
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject, 
                        callOnUpdate: (color) => { card.transform.localScale = new Vector3(color.r, color.g, color.b); }, 
                        new Color(card.transform.localScale.x, card.transform.localScale.y, card.transform.localScale.z),
                        new Color(defaultScale.x, defaultScale.y, defaultScale.z, 1),
                        hoverScaleTime);

        // Reset linked arrow color on fortune display
        if (linkedArrow != null) linkedArrow.ResetColor();
        if (peekedArrow != null) DestroyImmediate(peekedArrow.gameObject); peekedArrow = null;
    }

    public void SelectIngredient()
    {
        if (TutorialManager.Instance != null) TutorialManager.Instance.clickedCard = true;

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
