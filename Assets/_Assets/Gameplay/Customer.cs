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
        STRONGLYDISLIKE = 0,
        DISLIKE = 1,
        NEUTRAL = 2,
        LIKE = 3,
        LOVE = 4,
    }

    [Serializable]
    private struct FortunePreference
    {
        public Fortune fortune;
        public FORTUNEPREFERENCEENUM preference;
        public string dialogue;
    }

    [SerializeField] private List<FortunePreference> fortunePreferences = new();

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
    }

    public Fortune ReadFortune(List<Ingredient> ingredients)
    {
        Vector2 position = new Vector2();
        foreach(Ingredient ingredient in ingredients)
        {
            position += ingredient.fortuneOffset;
        }
        Debug.Log($"Current position: {position}");
        Fortune fortune = fortuneTable.ReadFortune(position);
        fortuneName.text = fortune.name;
        fortunePosition.text = position.ToString();
        Debug.Log(fortune.fortuneName);
        Debug.Log(ReactToFortune(fortune));

        return fortune;
    }

    private FORTUNEPREFERENCEENUM ReactToFortune(Fortune fortune)
    {
        foreach (FortunePreference fortunePreference in fortunePreferences)
        {
            if (fortunePreference.fortune == fortune)
            {
                SayDialogue(fortunePreference);
                return fortunePreference.preference;
            }
        }

        return FORTUNEPREFERENCEENUM.NEUTRAL;
    }

    private void SayDialogue(FortunePreference reaction)
    {
        dialogueDisplay.text = reaction.dialogue;
    }
}
