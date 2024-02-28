using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeDisplay : MonoBehaviour
{
    public Drink drink;

    [Header("Display References")]
    public TextMeshProUGUI recipeName;
    public TextMeshProUGUI recipeDescription;

    private void Awake()
    {
        recipeName.text = drink.drinkName;
        recipeDescription.text = drink.drinkDescription;
    }
}
