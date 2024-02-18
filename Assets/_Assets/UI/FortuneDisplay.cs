using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FortuneDisplay : Singleton<FortuneDisplay>
{
    public List<Fortune> displayedFortunes = new();
    private FortuneTable currentFortuneTable;
    public bool mouseInDisplay = false;

    [Header("Display References")]
    public TextMeshProUGUI fortuneName;
    public TextMeshProUGUI fortunePosition;
    public GameObject fortuneGrid;
    public GameObject fortuneRegionUIPrefab;
    public Camera camera;


    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Barista.Instance.customerChangeEvent.AddListener(GenerateFortuneRegionDisplay);
    }

    private void Update()
    {
        //DisplayMouseCoordinates();
    }


    public void GenerateFortuneRegionDisplay(Customer customer)
    {
        Debug.Log("Generating Fortune Region Display...");
        currentFortuneTable = customer.GetComponent<FortuneTable>();

        FortuneTable.FortuneRegion defaultRegion = new();
        defaultRegion.fortuneType = currentFortuneTable.defaultFortune;
        defaultRegion.topLeftBorder = new Vector2(0, 0);
        defaultRegion.bottomRightBorder = currentFortuneTable.fortuneTableSize;

        // Instantiate default region over entire table
        InstantiateFortuneRegion(defaultRegion, currentFortuneTable);

        // Instantiate all custom regions
        foreach (FortuneTable.FortuneRegion region in currentFortuneTable.fortuneRegions)
        {
            InstantiateFortuneRegion(region, currentFortuneTable);
        }
    }

    private void InstantiateFortuneRegion(FortuneTable.FortuneRegion region, FortuneTable fortuneTable)
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

        FortuneRegionUI fortuneRegionUI = newFortuneRegion.GetComponent<FortuneRegionUI>();
        fortuneRegionUI.fortune = region.fortuneType;
        newFortuneRegion.GetComponent<Image>().color = fortuneRegionUI.fortune.fortuneColor;
    }

    public void AddFortune(Fortune fortune)
    {
        displayedFortunes.Add(fortune);
        DisplayCurrentFortune();
    }

    public void RemoveFortune(Fortune fortune)
    {
        if (displayedFortunes.Contains(fortune))
        {
            displayedFortunes.Remove(fortune);
        }
        DisplayCurrentFortune();
    }

    public void DisplayCurrentFortune()
    {
        if (displayedFortunes.Count <= 0)
        {
            fortuneName.text = "";
            return;
        }

        Fortune displayedFortune = displayedFortunes[displayedFortunes.Count - 1];
        fortuneName.text = displayedFortune.name;
    }

    public void DisplayMouseCoordinates()
    {
        if (displayedFortunes.Count <= 0)
        {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(fortuneGrid.GetComponent<RectTransform>(), Input.mousePosition, camera, out Vector2 localPoint);
        Debug.Log(localPoint);

        /*
        Vector2 mousePos = Input.mousePosition;
        Debug.Log($"Raw mouse position: {mousePos}");
        // relative position 
        mousePos -= new Vector2(fortuneGrid.transform.position.x, fortuneGrid.transform.position.y);
        Debug.Log($"Relative mouse position: {mousePos}");
        Vector2 scaledMousePos = mousePos / fortuneGrid.GetComponent<RectTransform>().sizeDelta;
        scaledMousePos.y *= -1;
        Debug.Log($"Scaled mouse position: {scaledMousePos}");
        */

        fortunePosition.text = localPoint.ToString();
    }
}
