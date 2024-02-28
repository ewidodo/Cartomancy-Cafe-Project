using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneTable : MonoBehaviour
{
    [Serializable]
    public struct FortuneRegion
    {
        public string displayName;
        public Fortune fortuneType;
        public Vector2 topLeftBorder;
        public Vector2 bottomRightBorder;
    }

    [Header("Fortune table starts at (0, 0)")]
    public Vector2 fortuneTableSize = new();
    public Vector2 startingPosition = new();
    public List<FortuneRegion> fortuneRegions = new();
    public Fortune defaultFortune;

    private void Start()
    {
        InitFortunes();
    }

    // Throw an error if any inspector-defined fortunes are out of bounds of the fortune table
    private void InitFortunes()
    {
        if (fortuneTableSize == null || fortuneTableSize.x <= 0 || fortuneTableSize.y <= 0)
        {
            Debug.LogError("Fortune table size undefined or 0!");
        }

        foreach (FortuneRegion region in fortuneRegions)
        {
            if (region.topLeftBorder.x < 0 || region.topLeftBorder.y < 0 || 
                region.topLeftBorder.x > fortuneTableSize.x || region.topLeftBorder.y > fortuneTableSize.y ||
                region.bottomRightBorder.x < 0 || region.bottomRightBorder.y < 0 || 
                region.bottomRightBorder.x > fortuneTableSize.x || region.bottomRightBorder.y > fortuneTableSize.y)
            {
                Debug.LogError("Fortune overflow error! Fortune \"" + region.fortuneType + "\" out of bounds!");
            }
        }
    }

    public Fortune ReadFortune(Vector2 position)
    {
        // Check fortune bounds
        if (position.x < 0 || position.y < 0 || 
            position.x > fortuneTableSize.x || position.y > fortuneTableSize.y)
        {
            Debug.LogWarning($"Fortune out of bounds error! Fortune position {position} " +
                           $"outside the defined range of (0, 0) to {fortuneTableSize} of the fortune table.");
            return defaultFortune;
        }

        // Match position to relevant fortune. Note that this prioritizes fortunes higher in the list if multiple overlap
        foreach (FortuneRegion region in fortuneRegions)
        {
            if (position.x >= region.topLeftBorder.x && position.y >= region.topLeftBorder.y && 
                position.x <= region.bottomRightBorder.x && position.y <= region.bottomRightBorder.y)
            {
                return region.fortuneType;
            }
        }

        // No fortunes match position given
        Debug.LogWarning("No fortunes found for fortune position (" + position.x + ", " + position.y + ").");
        return defaultFortune;
    }
}
