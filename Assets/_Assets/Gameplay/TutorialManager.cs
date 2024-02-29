using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : SingletonDontDestroy<TutorialManager>
{
    public float inactivityDelay;

    [Header("Booleans")]
    [ReadOnly] public bool clickedCard = false;
    [ReadOnly] public bool hoveredFortunes = false;
    [ReadOnly] public bool readRecipeBook = false;
    [ReadOnly] public bool servedDrink = false;
    public bool seenTutorial = false;

    [Header("Display References")]
    public GameObject cardTutorialText;
    public GameObject fortuneTutorialText;
    public GameObject recipeBookTutorialText;
    public GameObject serveDrinkTutorialText;

    new private void Awake()
    {
        base.Awake();
        HideAllTutorialText();
    }

    private void Start()
    {
        //StartCoroutine(CardTutorial());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void HideAllTutorialText()
    {
        cardTutorialText.SetActive(false);
        fortuneTutorialText.SetActive(false);
        recipeBookTutorialText.SetActive(false);
        serveDrinkTutorialText.SetActive(false);
        StopAllCoroutines();
    }

    public IEnumerator CardTutorial()
    {
        if (!clickedCard) yield return new WaitForSeconds(inactivityDelay);

        if (!clickedCard) cardTutorialText.SetActive(true);

        while (!clickedCard)
        {
            yield return null;
        }

        cardTutorialText.SetActive(false);
        StartCoroutine(FortuneTutorial());
        yield break;
    }

    public IEnumerator FortuneTutorial()
    {
        if (!hoveredFortunes) yield return new WaitForSeconds(inactivityDelay);

        if (!hoveredFortunes) fortuneTutorialText.SetActive(true);

        while (!hoveredFortunes)
        {
            yield return null;
        }

        fortuneTutorialText.SetActive(false);
        StartCoroutine(RecipeTutorial());
        yield break;
    }

    private IEnumerator RecipeTutorial()
    {
        if (!readRecipeBook) yield return new WaitForSeconds(inactivityDelay);

        if (!readRecipeBook) recipeBookTutorialText.SetActive(true);

        while (!readRecipeBook)
        {
            yield return null;
        }

        recipeBookTutorialText.SetActive(false);
        StartCoroutine(DrinkTutorial());
        yield break;
    }

    private IEnumerator DrinkTutorial()
    {
        if (!servedDrink) yield return new WaitForSeconds(inactivityDelay);

        if (!servedDrink) serveDrinkTutorialText.SetActive(true);

        while (!servedDrink)
        {
            yield return null;
        }

        serveDrinkTutorialText.SetActive(false);
        yield break;
    }
}
