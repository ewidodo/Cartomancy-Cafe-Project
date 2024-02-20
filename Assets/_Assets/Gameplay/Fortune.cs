using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fortune", menuName = "Fortunes/Fortune", order = 0)]
public class Fortune : ScriptableObject
{
    public string fortuneName;
    [TextArea(3, 5)] public string fortuneDescription;
    public Sprite fortuneSprite;
    public Color fortuneColor;
}
