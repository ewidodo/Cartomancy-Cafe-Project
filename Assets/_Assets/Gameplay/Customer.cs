using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private List<Ingredient> drinkIngredients = new();
    private FortuneTable fortuneTable;

    private void Awake()
    {
        fortuneTable = GetComponent<FortuneTable>();
    }

    public void GiveIngredients(List<Ingredient> ingredients)
    {
        drinkIngredients = ingredients;
        Fortune fortune = ReadFortune(ingredients);
        Debug.Log(fortune.fortuneName);
    }

    public Fortune ReadFortune(List<Ingredient> ingredients)
    {
        Vector2 position = new Vector2();
        foreach(Ingredient ingredient in ingredients)
        {
            position += ingredient.fortuneOffset;
        }
        Debug.Log($"Current position: {position}");

        return fortuneTable.ReadFortune(position);
    }
}
