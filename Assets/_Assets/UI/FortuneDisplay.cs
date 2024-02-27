using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FortuneDisplay : Singleton<FortuneDisplay>
{
    public List<Fortune> hoveredFortunes = new();
    public Fortune currentDrinkFortune;
    private FortuneTable currentFortuneTable;
    public bool mouseInDisplay = false;

    [Header("Display References")]
    public TextMeshProUGUI fortuneName;
    public TextMeshProUGUI fortuneDescription;
    public Image fortuneSprite;
    public GameObject fortuneGrid;
    public GameObject fortuneRegionUIPrefab;
    public Camera camera;
    public Transform arrowPrefab;
    public Transform arrowParent;


    private new void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        // Regenerate display whenever a new customer appears
        Barista.Instance.customerChangeEvent.AddListener(GenerateFortuneRegionDisplay);
    }

    private void Update()
    {
        //DisplayMouseCoordinates();
    }

    public void DisplayFortune(Fortune fortune)
    {
        if (fortune == null)
        {
            fortuneName.text = "";
            fortuneDescription.text = "";
            fortuneSprite.sprite = null;
        }

        fortuneName.text = fortune.name;
        fortuneDescription.text = fortune.fortuneDescription;
        fortuneSprite.sprite = fortune.fortuneSprite;
    }

    #region Arrow Calculations
    public Arrow DisplayVector(Vector2 oldPos, Vector2 newPos)
    {
        Vector3 one = FortuneDisplayToScreenCoordinates(Vector3.one);

        Debug.Log($"Vector points: {oldPos} and {newPos}");
        // Instantiate new arrow prefab
        Transform arrow = Instantiate(arrowPrefab, arrowParent);

        // Scale tail to distance between the two points
        float dist = Vector2.Distance(oldPos, newPos);
        Debug.Log($"Vector distance: {dist}");
        RectTransform arrowRect = arrow.GetChild(2).GetComponent<RectTransform>();
        arrowRect.sizeDelta = new Vector2(dist * one.x, arrowRect.sizeDelta.y);

        // Rotate to the angle between the two points
        float rot = Vector2.Angle(new Vector2(1, 0), newPos - oldPos); // y axis is flipped
        if (newPos.y > oldPos.y) rot *= -1;
        Debug.Log($"Vector rotation: {rot}");
        arrow.localRotation = Quaternion.Euler(0, 0, rot);

        // Move arrow to newPos (anchor is at tip)
        arrow.localPosition = FortuneDisplayToScreenCoordinates(newPos);

        return arrow.GetComponent<Arrow>();
    }

    public void ClearArrows()
    {
        while (arrowParent.childCount > 0)
        {
            DestroyImmediate(arrowParent.GetChild(0).gameObject);
        }
    }

    private Vector3 FortuneDisplayToScreenCoordinates(Vector3 worldPos)
    {
        RectTransform fortuneGridRect = fortuneGrid.GetComponent<RectTransform>();

        // There's no way this just works right
        float gridScalarX = fortuneGridRect.sizeDelta.x / currentFortuneTable.fortuneTableSize.x;
        float gridScalarY = fortuneGridRect.sizeDelta.y / currentFortuneTable.fortuneTableSize.y;

        return new Vector3(worldPos.x * gridScalarX, -1 * worldPos.y * gridScalarY, 1);
    }
    #endregion

    #region Initial Display Generation
    public void GenerateFortuneRegionDisplay(Customer customer)
    {
        Debug.Log("Generating Fortune Region Display...");

        // Clear current grid
        while (fortuneGrid.transform.childCount > 0)
        {
            DestroyImmediate(fortuneGrid.transform.GetChild(0).gameObject);
        }

        ClearArrows();

        currentFortuneTable = customer.GetComponent<FortuneTable>();

        FortuneTable.FortuneRegion defaultRegion = new();
        defaultRegion.fortuneType = currentFortuneTable.defaultFortune;
        defaultRegion.topLeftBorder = new Vector2(0, 0);
        defaultRegion.bottomRightBorder = currentFortuneTable.fortuneTableSize;

        // Instantiate default region over entire table
        InstantiateFortuneRegion(defaultRegion, currentFortuneTable, invisible:true);

        // Instantiate all custom regions
        foreach (FortuneTable.FortuneRegion region in currentFortuneTable.fortuneRegions)
        {
            InstantiateFortuneRegion(region, currentFortuneTable);
        }
    }

    private void InstantiateFortuneRegion(FortuneTable.FortuneRegion region, FortuneTable fortuneTable, bool invisible=false)
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

        if (invisible) newFortuneRegion.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        else newFortuneRegion.GetComponent<Image>().color = fortuneRegionUI.fortune.fortuneColor;
    }
    #endregion

    #region Hover Behavior
    public void AddHoveredFortune(Fortune fortune)
    {
        hoveredFortunes.Add(fortune);
        DisplayHoveredFortune();
    }

    public void RemoveHoveredFortune(Fortune fortune)
    {
        if (hoveredFortunes.Contains(fortune))
        {
            hoveredFortunes.Remove(fortune);
        }
        DisplayHoveredFortune();
    }

    public void DisplayHoveredFortune()
    {
        if (hoveredFortunes.Count <= 0)
        {
            if (currentDrinkFortune == null) DisplayFortune(null);
            else DisplayFortune(currentDrinkFortune);
            return;
        }

        Fortune displayedFortune = hoveredFortunes[hoveredFortunes.Count - 1];
        DisplayFortune(displayedFortune);
    }

    public void DisplayMouseCoordinates()
    {
        if (hoveredFortunes.Count <= 0)
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

        //fortunePosition.text = localPoint.ToString();
    }
    #endregion
}
