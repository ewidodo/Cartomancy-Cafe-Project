using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    public RectTransform tail;
    public Color defaultColor;
    public Color highlightColor;

    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void ChangeColor(Color color)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().color = color;
        }
    }

    public void Highlight()
    {
        canvas.sortingOrder += 1;
        ChangeColor(highlightColor);
    }

    public void ResetColor()
    {
        canvas.sortingOrder -= 1;
        ChangeColor(defaultColor);
    }
}
