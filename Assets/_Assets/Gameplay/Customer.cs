using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Customer : MonoBehaviour
{
    [ReadOnly] public bool customerAcceptingDrink = true;

    private Drink drink;
    private Ingredient addon;
    private Ingredient mystery;

    [SerializeField, ReadOnly] private List<Ingredient> preferredIngredients = new();
    [HideInInspector] public List<IngredientCard> drinkIngredients = new();
    private FortuneTable fortuneTable;

    [Header("Timing")]
    public float spawnTime;

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
    public float textScrollRate = 20;
    public float periodPauseTime = 0.25f;
    public float commaPauseTime = 0.1f;
    public float nextDialogueDelay = 2;
    private Coroutine currentDialogueRoutine;

    [Header("Display References")]
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI dialogueDisplay;
    public GameObject dialogueBubble;

    [Header("Data References")]
    public List<Fortune> fortunes;
    public List<Drink> drinks;
    public List<Ingredient> ingredients;

    private void Awake()
    {
        fortuneTable = GetComponent<FortuneTable>();
        GeneratePreferenceDictionary();
        //GenerateDesires();
        customerAcceptingDrink = true;
        dialogueBubble.SetActive(false);
        transform.localPosition = new Vector3(1000, 0, 0);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
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

        //drinkDialogue = $"Give me a {drink.drinkName} with {addon.ingredientName} and make it {mystery.ingredientKeyword}";

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
        Vector2 position = fortuneTable.startingPosition;

        foreach (Ingredient ingredient in ingredients)
        {
            position += ingredient.fortuneOffset;
        }

        Fortune fortune = fortuneTable.ReadFortune(position);

        return fortune;
    }

    public Fortune DisplayFortune(List<IngredientCard> ingredients)
    {
        Vector2 position = fortuneTable.startingPosition;

        FortuneDisplay.Instance.ClearArrows();

        foreach(IngredientCard ingredientCard in ingredients)
        {
            Vector2 oldPosition = position;
            position += ingredientCard.ingredient.fortuneOffset;
            //position = new Vector2(Mathf.Clamp(position.x, 0f, fortuneTable.fortuneTableSize.x),
            //                       Mathf.Clamp(position.y, 0f, fortuneTable.fortuneTableSize.y));
            ingredientCard.linkedArrow = FortuneDisplay.Instance.DisplayVector(oldPosition, position);
            ingredientCard.linkedArrow.linkedIngredientCard = ingredientCard;
        }

        Fortune fortune = fortuneTable.ReadFortune(position);

        if (ingredients.Count <= 0)
        {
            FortuneDisplay.Instance.currentDrinkFortune = null;
            FortuneDisplay.Instance.DisplayFortune(null);
        }
        else
        {
            FortuneDisplay.Instance.currentDrinkFortune = fortune;
            FortuneDisplay.Instance.DisplayFortune(fortune);
        }

        return fortune;
    }

    public Fortune PeekFortune(IngredientCard card)
    {
        Vector2 position = fortuneTable.startingPosition;
        Vector2 oldPosition;

        FortuneDisplay.Instance.ClearArrows();

        // Display current fortune
        foreach (IngredientCard ingredientCard in drinkIngredients)
        {
            oldPosition = position;
            position += ingredientCard.ingredient.fortuneOffset;
            //position = new Vector2(Mathf.Clamp(position.x, 0f, fortuneTable.fortuneTableSize.x),
            //                       Mathf.Clamp(position.y, 0f, fortuneTable.fortuneTableSize.y));
            ingredientCard.linkedArrow = FortuneDisplay.Instance.DisplayVector(oldPosition, position);
            ingredientCard.linkedArrow.linkedIngredientCard = ingredientCard;
        }

        // Peek next vector
        oldPosition = position;
        position += card.ingredient.fortuneOffset;
        //position = new Vector2(Mathf.Clamp(position.x, 0f, fortuneTable.fortuneTableSize.x),
        //                       Mathf.Clamp(position.y, 0f, fortuneTable.fortuneTableSize.y));
        card.peekedArrow = FortuneDisplay.Instance.DisplayVector(oldPosition, position);
        card.peekedArrow.linkedIngredientCard = card;
        card.peekedArrow.Highlight();

        // Display fortune
        Fortune fortune = fortuneTable.ReadFortune(position);
        FortuneDisplay.Instance.currentDrinkFortune = fortune;
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
            yield return new WaitForSeconds(1 / textScrollRate);
            if (c == '.' || c == '?' || c == '!') yield return new WaitForSeconds(periodPauseTime);
            if (c == ',') yield return new WaitForSeconds(commaPauseTime);
        }

        if (callback != null) callback.Invoke();
        currentDialogueRoutine = null;
        yield return null;
    }

    private string ParseWildcards(string text)
    {
        string[] split = text.Split(' ');
        for (int i = 0; i < split.Length; i++)
        {
            string s = split[i];

            if (s[0] == '$')
            {
                string wildcard = s.Substring(1);

                if (wildcard.Substring(0, 5) == "drink")
                {
                    split[i] = drink.drinkName + wildcard.Substring(5);

                    // Replace "a" with "an" for vowels
                    if ((split[i][0] == 'A' || split[i][0] == 'X') && split[i - 1][split[i-1].Length - 1] == 'a')
                    {
                        split[i - 1] += "n";
                    }

                    // Remove extra "the"s
                    if (split[i][0] == 'T' && split[i - 1] == "the")
                    {
                        split[i] = split[i].Substring(4);
                    }
                }
                else if (wildcard.Substring(0, 5) == "addon")
                {
                    split[i] = addon.ingredientName + wildcard.Substring(5);

                    // Special case for "Reversed"
                    if (split[i] == "Reversed" && split[i - 1] == "with")
                    {
                        split[i - 1] = "";
                    }
                }
                else if (wildcard.Substring(0, 7) == "mystery")
                {
                    split[i] = mystery.ingredientKeyword + wildcard.Substring(7);
                }
            }
        }

        // recombine split
        string result = "";
        for (int i = 0; i < split.Length; ++i)
        {
            if (i > 0 && split[i] != "") result += " ";
            result += split[i];
        }

        return result;
    }

    private void IncrementDialogueState()
    {

    }

    private void RespondToFortune(FortunePreference reaction)
    {
        if (currentDialogueRoutine != null) StopCoroutine(currentDialogueRoutine); currentDialogueRoutine = null;
        ScoreManager.Instance.AddScore((int) reaction.preference);
        customerAcceptingDrink = false;
        StartCoroutine(TextScroll(preferenceResponses[reaction.preference], CustomerManager.Instance.SwapCustomers));
    }

    private void DisplayDrinkDialogue()
    {
        if (currentDialogueRoutine != null) StopCoroutine(currentDialogueRoutine); currentDialogueRoutine = null;
        currentDialogueRoutine = StartCoroutine(TextScroll(drinkDialogue, null));
    }

    public void Spawn()
    {
        GenerateDesires();
        SpawnAnimation(TriggerSpawnDialogue);
    }

    public void SpawnAnimation(Action callback)
    {
        LeanTween.value(this.gameObject,
                        callOnUpdate: (val) => { transform.localPosition = new Vector3(val, transform.localPosition.y, transform.localPosition.z); },
                        1000,
                        0,
                        spawnTime)
                        .setOnComplete(callback);
    }

    public void TriggerSpawnDialogue()
    {
        StartCoroutine(SpawnDialogue());
    }

    public IEnumerator SpawnDialogue()
    {
        dialogueBubble.SetActive(true);
        if (currentDialogueRoutine != null) StopCoroutine(currentDialogueRoutine);
        yield return currentDialogueRoutine = StartCoroutine(TextScroll(greetingDialogue, null));
        yield return new WaitForSeconds(nextDialogueDelay);
        if (currentDialogueRoutine == null && customerAcceptingDrink) yield return currentDialogueRoutine = StartCoroutine(TextScroll(ParseWildcards(drinkDialogue), null));

        if (TutorialManager.Instance != null && TutorialManager.Instance.clickedCard == false)
        {
            StartCoroutine(TutorialManager.Instance.CardTutorial());
        }
    }

    public void Despawn()
    {
        dialogueBubble.SetActive(false);
        DespawnAnimation(DestroySelf);
    }

    public void DespawnAnimation(Action callback)
    {
        LeanTween.value(this.gameObject,
                        callOnUpdate: (val) => { transform.localPosition = new Vector3(val, transform.localPosition.y, transform.localPosition.z); },
                        0,
                        1000,
                        spawnTime)
                        .setOnComplete(callback);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
