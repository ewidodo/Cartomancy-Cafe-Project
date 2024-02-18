using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FortuneDisplay : Singleton<FortuneDisplay>
{
    public List<Fortune> displayedFortunes = new();

    [Header("Display References")]
    public TextMeshProUGUI fortuneName;
    public TextMeshProUGUI fortunePosition;
    public GameObject fortuneGrid;
    public GameObject fortuneRegionUIPrefab;


    private void Start()
    {
        Barista.Instance.customerChangeEvent.AddListener(GenerateFortuneRegionDisplay);
    }


    public void GenerateFortuneRegionDisplay(Customer customer)
    {
        Debug.Log("Generating Fortune Region Display...");

        FortuneTable fortuneTable = customer.GetComponent<FortuneTable>();
        foreach(FortuneTable.FortuneRegion region in fortuneTable.fortuneRegions)
        {
            GameObject newFortuneRegion = Instantiate(fortuneRegionUIPrefab, fortuneGrid.transform);
            RectTransform rect = newFortuneRegion.GetComponent<RectTransform>();
            RectTransform fortuneGridRect = fortuneGrid.GetComponent<RectTransform>();

            // There's no way this just works right
            float gridScalarX = fortuneGridRect.sizeDelta.x / fortuneTable.fortuneTableSize.x;
            float gridScalarY = fortuneGridRect.sizeDelta.y / fortuneTable.fortuneTableSize.y;
            rect.sizeDelta = new Vector3((region.bottomRightBorder.x - region.topLeftBorder.x) * gridScalarX, 
                                         (region.bottomRightBorder.y - region.topLeftBorder.y) * gridScalarY,
                                         1);
            rect.localPosition = new Vector3(region.topLeftBorder.x * gridScalarX, 
                                             -1 * region.topLeftBorder.y * gridScalarY, 
                                             1);
            newFortuneRegion.GetComponent<Image>().color = Random.ColorHSV();
            // You might have to fuck with the collider size as well :sob:
        }
    }

    public void AddFortune(Fortune fortune)
    {
        displayedFortunes.Add(fortune);
    }

    public void RemoveFortune(Fortune fortune)
    {
        if (displayedFortunes.Contains(fortune))
        {
            displayedFortunes.Remove(fortune);
        }
    }

    public void DisplayCurrentFortune()
    {
        if (displayedFortunes.Count <= 0)
        {
            // display nothing or default
            return;
        }

        // display displayedFortunes[displayedFortunes.Count - 1]
    }
}
