using UnityEngine;
using System;

/// <summary>
/// Displays a property/field if a bool condition is met
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class DisplayIfBoolAttribute : PropertyAttribute
{
    public string boolToCheck {get; private set;}
    public bool conditionToMeet {get; private set;}

    public DisplayIfBoolAttribute(string _boolToCheck, bool _conditionToMeet = true)
    {
        boolToCheck = _boolToCheck;
        conditionToMeet = _conditionToMeet;
    }
}
