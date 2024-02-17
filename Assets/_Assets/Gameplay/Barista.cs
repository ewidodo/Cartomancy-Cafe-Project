using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barista : MonoBehaviour
{
    [HideInInspector] public List<Ingredient> currentDrinkIngredients = new();
    public List<Ingredient> reserveIngredients = new();
    public Customer currentCustomer;

    private void Start()
    {
        UseIngredient(reserveIngredients[0]);
        UseIngredient(reserveIngredients[1]);
        UseIngredient(reserveIngredients[2]);
        GiveCustomerDrink();
    }

    public void GiveCustomerDrink()
    {
        currentCustomer.GiveIngredients(currentDrinkIngredients);
    }

    public void UseIngredient(Ingredient ingredient)
    {
        currentDrinkIngredients.Add(ingredient);
        Debug.Log(currentCustomer.ReadFortune(currentDrinkIngredients));
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        if (currentDrinkIngredients.Contains(ingredient))
        {
            currentDrinkIngredients.Remove(ingredient);
            Debug.Log(currentCustomer.ReadFortune(currentDrinkIngredients));
        }
        else
        {
            Debug.LogError($"No ingredient of type {ingredient.name} found in current drink ingredients!");
        }
    }
}
