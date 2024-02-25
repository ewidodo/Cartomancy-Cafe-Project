using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    private Drink drink;
    private Ingredient addon;
    private Ingredient mystery;

    [SerializeField, ReadOnly] private List<Ingredient> preferredIngredients = new();
    private List<IngredientCard> drinkIngredients = new();
    private FortuneTable fortuneTable;

    [Serializable]
    private enum FORTUNEPREFERENCEENUM
    {
        NEGATIVE = 0,
        NEUTRAL = 1,
        POSITIVE = 2,
    }

    [Serializable]
    private struct FortunePreference
    {
        public Fortune fortune;
        public FORTUNEPREFERENCEENUM preference;
        //public string dialogue;
    }

    [SerializeField] private List<FortunePreference> fortunePreferences = new();

    [Header("Dialogue")]
    [TextArea(1, 5)] public string greetingDialogue;
    [TextArea(1, 5)] public string drinkDialogue;
    [TextArea(1, 5)] public string positiveDialogue;
    [TextArea(1, 5)] public string neutralDialogue;
    [TextArea(1, 5)] public string negativeDialogue;
    private Dictionary<FORTUNEPREFERENCEENUM, string> preferenceResponses = new();
    public float textScrollInterval = 0.05f;

    [Header("Display References")]
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI dialogueDisplay;

    [Header("Data References")]
    public List<Fortune> fortunes;
    public List<Drink> drinks;
    public List<Ingredient> ingredients;

    private void Awake()
    {
        fortuneTable = GetComponent<FortuneTable>();
        GeneratePreferenceDictionary();
        GenerateDesires();
    }

    private void GeneratePreferenceDictionary()
    {
        preferenceResponses.Add(FORTUNEPREFERENCEENUM.POSITIVE, positiveDialogue);
        preferenceResponses.Add(FORTUNEPREFERENCEENUM.NEUTRAL, neutralDialogue);
        preferenceResponses.Add(FORTUNEPREFERENCEENUM.NEGATIVE, negativeDialogue);
    }

    // Generate base drink, modifier ingredients, mystery ingredients
    private void GenerateDesires()
    {
        // Generate base drink
        drink = drinks[UnityEngine.Random.Range(0, drinks.Count)];

        // Generate specific ingredient
        addon = ingredients[UnityEngine.Random.Range(0, ingredients.Count)];

        // Generate mystery ingredient
        mystery = ingredients[UnityEngine.Random.Range(0, ingredients.Count)];

        // Generate preferred fortune
        preferredIngredients.AddRange(drink.ingredients);
        preferredIngredients.Add(addon);
        preferredIngredients.Add(mystery);
        Fortune preferredFortune = ReadFortune(preferredIngredients);
        FortunePreference preference = new();
        preference.fortune = preferredFortune;
        preference.preference = FORTUNEPREFERENCEENUM.POSITIVE;
        fortunePreferences.Add(preference);

        // Generate other fortune preferences randomly?
        foreach (FortuneTable.FortuneRegion fortuneRegion in fortuneTable.fortuneRegions)
        {
            FortunePreference tablePreference = new();
            tablePreference.fortune = fortuneRegion.fortuneType;
            tablePreference.preference = (FORTUNEPREFERENCEENUM) UnityEngine.Random.Range(0, 2);
            fortunePreferences.Add(tablePreference);
        }

        drinkDialogue = $"Give me a {drink.drinkName} with {addon.ingredientName} and make it {mystery.ingredientKeyword}";

        // Double check everything?
    }

    public void GiveIngredients(List<IngredientCard> ingredients)
    {
        drinkIngredients = ingredients;
        Fortune fortune = DisplayFortune(ingredients);
        FortunePreference preference =  ReactToFortune(fortune);
        RespondToFortune(preference);
    }

    public Fortune ReadFortune(List<Ingredient> ingredients)
    {
        Vector2 position = new Vector2();

        foreach (Ingredient ingredient in ingredients)
        {
            position += ingredient.fortuneOffset;
        }

        Fortune fortune = fortuneTable.ReadFortune(position);

        return fortune;
    }

    public Fortune DisplayFortune(List<IngredientCard> ingredients)
    {
        Vector2 position = new Vector2();

        foreach(IngredientCard ingredient in ingredients)
        {
            Vector2 oldPosition = position;
            position += ingredient.fortuneOffset;
            position = new Vector2(Mathf.Clamp(position.x, 0f, fortuneTable.fortuneTableSize.x),
                                   Mathf.Clamp(position.y, 0f, fortuneTable.fortuneTableSize.y));
            FortuneDisplay.Instance.DisplayVector(oldPosition, position);
        }

        Fortune fortune = fortuneTable.ReadFortune(position);
        FortuneDisplay.Instance.DisplayFortune(fortune);

        return fortune;
    }

    private FortunePreference ReactToFortune(Fortune fortune)
    {
        foreach (FortunePreference fortunePreference in fortunePreferences)
        {
            if (fortunePreference.fortune == fortune)
            {
                return fortunePreference;
            }
        }

        Debug.LogWarning("Customer does not have a preference for the given fortune! Defaulting to neutral...");
        FortunePreference defaultPreference = new FortunePreference();
        defaultPreference.preference = FORTUNEPREFERENCEENUM.NEUTRAL;
        return defaultPreference;
    }

    private IEnumerator TextScroll(string finalText, Action callback)
    {
        dialogueDisplay.text = "";

        foreach (char c in finalText)
        {
            dialogueDisplay.text += c;
            yield return new WaitForSeconds(textScrollInterval);
        }

        if (callback != null) callback.Invoke();
        yield return null;
    }

    private void IncrementDialogueState()
    {

    }

    private void RespondToFortune(FortunePreference reaction)
    {
        StartCoroutine(TextScroll(preferenceResponses[reaction.preference], CustomerManager.Instance.SwapCustomers));
    }

    public void Spawn()
    {
        StartCoroutine(TextScroll(greetingDialogue, null));
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}
