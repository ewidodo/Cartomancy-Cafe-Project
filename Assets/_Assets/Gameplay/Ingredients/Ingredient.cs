using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Ingredients/Ingredient", order = 0)]
public class Ingredient : ScriptableObject
{
    public string ingredientName;
    public string ingredientKeyword;
    public Vector2 fortuneOffset;
    [TextArea(3, 5)] public string ingredientDescription;
    public Sprite ingredientSprite;
    public string ingredientNumber;
    //public Color ingredientColor;
}
