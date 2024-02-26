using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drink", menuName = "Drinks/Drink", order = 0)]
public class Drink : ScriptableObject
{
    public string drinkName;
    [TextArea(3, 5)] public string drinkDescription;
    public Sprite drinkSprite;
    public List<Ingredient> ingredients;
    public Color drinkColor;
}
