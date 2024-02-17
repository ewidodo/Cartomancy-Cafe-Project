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
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI descriptionDisplay;
    public TextMeshProUGUI fortuneOffsetDisplay;


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
