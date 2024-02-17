using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barista : Singleton<Barista>
{
    [HideInInspector] public List<Ingredient> currentDrinkIngredients = new();
    public List<Ingredient> reserveIngredients = new();
    public Customer currentCustomer;

    [Header("Display References")]
    public GameObject currentIngredientDisplay;

    private void Start()
    {
        //UseIngredient(reserveIngredients[0]);
        //UseIngredient(reserveIngredients[1]);
        //UseIngredient(reserveIngredients[2]);
        //GiveCustomerDrink();
    }

    public void GiveCustomerDrink()
    {
        currentCustomer.GiveIngredients(currentDrinkIngredients);
        foreach(Ingredient ingredient in currentDrinkIngredients)
        {
            Destroy(ingredient.gameObject);
        }
        currentDrinkIngredients.Clear();
    }

    public void UseIngredient(Ingredient ingredient)
    {
        // Instantiate ingredient in current ingredient display
        Ingredient addedIngredient = Instantiate(ingredient.gameObject, currentIngredientDisplay.transform).GetComponent<Ingredient>();
        addedIngredient.inDrink = true;

        currentDrinkIngredients.Add(addedIngredient);
        currentCustomer.ReadFortune(currentDrinkIngredients);
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        if (currentDrinkIngredients.Contains(ingredient))
        {
            currentDrinkIngredients.Remove(ingredient);
            Destroy(ingredient.gameObject);
            currentCustomer.ReadFortune(currentDrinkIngredients);
        }
        else
        {
            Debug.LogError($"No ingredient of type {ingredient.name} found in current drink ingredients!");
        }
    }
}
