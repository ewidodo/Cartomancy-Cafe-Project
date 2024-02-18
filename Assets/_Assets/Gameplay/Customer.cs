using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    private List<Ingredient> drinkIngredients = new();
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
        public string dialogue;
    }

    [SerializeField] private List<FortunePreference> fortunePreferences = new();

    [Header("Dialogue")]
    [TextArea(1, 5)] public string greetingDialogue;

    [Header("Display References")]
    public TextMeshProUGUI dialogueDisplay;
    public TextMeshProUGUI fortuneName;
    public TextMeshProUGUI fortunePosition;

    private void Awake()
    {
        fortuneTable = GetComponent<FortuneTable>();
    }

    public void GiveIngredients(List<Ingredient> ingredients)
    {
        drinkIngredients = ingredients;
        Fortune fortune = ReadFortune(ingredients);
        FortunePreference preference =  ReactToFortune(fortune);
        SayDialogue(preference);
        CustomerManager.Instance.SwapCustomers();
    }

    public Fortune ReadFortune(List<Ingredient> ingredients)
    {
        Vector2 position = new Vector2();
        foreach(Ingredient ingredient in ingredients)
        {
            position += ingredient.fortuneOffset;
            position = new Vector2(Mathf.Clamp(position.x, 0f, fortuneTable.fortuneTableSize.x),
                                   Mathf.Clamp(position.y, 0f, fortuneTable.fortuneTableSize.y));
        }
        Fortune fortune = fortuneTable.ReadFortune(position);
        FortuneDisplay.Instance.fortuneName.text = fortune.name;
        FortuneDisplay.Instance.fortunePosition.text = position.ToString();

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
        dialogueDisplay.text = reaction.dialogue;
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
