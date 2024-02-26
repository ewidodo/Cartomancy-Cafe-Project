using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Barista : Singleton<Barista>
{
    [HideInInspector] public List<IngredientCard> currentDrinkIngredients = new();
    public List<IngredientCard> reserveIngredients = new();
    public Customer currentCustomer;
    public UnityEvent<Customer> customerChangeEvent;

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
        if (currentCustomer == null)
        {
            return;
        }

        currentCustomer.GiveIngredients(currentDrinkIngredients);
        foreach(IngredientCard ingredient in currentDrinkIngredients)
        {
            Destroy(ingredient.gameObject);
        }
        currentDrinkIngredients.Clear();
    }

    public void UseIngredient(IngredientCard ingredient)
    {
        if (!currentCustomer.customerAcceptingDrink)
        {
            return;
        }


        // Instantiate ingredient in current ingredient display
        IngredientCard addedIngredient = Instantiate(ingredient.gameObject, currentIngredientDisplay.transform).GetComponent<IngredientCard>();
        addedIngredient.inDrink = true;
        addedIngredient.defaultScale = new Vector3(0.5f, 0.5f, 0.5f);
        addedIngredient.card.transform.localScale = addedIngredient.defaultScale;

        currentDrinkIngredients.Add(addedIngredient);
        currentCustomer.drinkIngredients.Add(addedIngredient);
        currentCustomer.DisplayFortune(currentDrinkIngredients);
    }

    public void RemoveIngredient(IngredientCard ingredient)
    {
        if (currentDrinkIngredients.Contains(ingredient))
        {
            currentDrinkIngredients.Remove(ingredient);
            currentCustomer.drinkIngredients.Remove(ingredient);
            Destroy(ingredient.gameObject);
            currentCustomer.DisplayFortune(currentDrinkIngredients);
        }
        else
        {
            Debug.LogError($"No ingredient of type {ingredient.name} found in current drink ingredients!");
        }
    }
}
