using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    [SerializeField] private List<FortunePreference> fortunePreferences = new();

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
                return fortunePreference.preference;
            }
        }

        return FORTUNEPREFERENCEENUM.NEUTRAL;
    }
}
