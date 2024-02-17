using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// The data component of anything that will affect the final fortune
public class Ingredient : MonoBehaviour
{
    public string name;
    public string description;
    public Vector2 fortuneOffset = new();

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
        Barista.Instance?.UseIngredient(this);
    }
}
