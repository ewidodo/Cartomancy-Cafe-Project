using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
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
    [TextArea(1, 5)] public string positiveDialogue;
    [TextArea(1, 5)] public string neutralDialogue;
    [TextArea(1, 5)] public string negativeDialogue;
    private Dictionary<FORTUNEPREFERENCEENUM, string> preferenceResponses = new();

    [Header("Display References")]
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI dialogueDisplay;

    private void Awake()
    {
        fortuneTable = GetComponent<FortuneTable>();
        GeneratePreferenceDictionary();
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

        // Generate specific ingredient

        // Generate mystery ingredient

        // Generate fortune preferences
        foreach (FortuneTable.FortuneRegion fortuneRegion in fortuneTable.fortuneRegions)
        {

        }

        // Double check everything
    }

    public void GiveIngredients(List<IngredientCard> ingredients)
    {
        drinkIngredients = ingredients;
        Fortune fortune = ReadFortune(ingredients);
        FortunePreference preference =  ReactToFortune(fortune);
        SayDialogue(preference);
        CustomerManager.Instance.SwapCustomers();
    }

    public Fortune ReadFortune(List<IngredientCard> ingredients)
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

    private void SayDialogue(FortunePreference reaction)
    {
        dialogueDisplay.text = preferenceResponses[reaction.preference];
    }

    public void Spawn()
    {
        dialogueDisplay.text = greetingDialogue;
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}
