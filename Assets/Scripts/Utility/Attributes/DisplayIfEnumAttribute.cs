using UnityEngine;
using System;

/// <summary>
/// Displays a property/field if an enum condition is met
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class DisplayIfEnumAttribute : PropertyAttribute
{
    public string enumToCheck {get; private set;}
    public object conditionToMeet {get; private set;}

    public DisplayIfEnumAttribute(string _enumToCheck, object _conditionToMeet)
    {
        enumToCheck = _enumToCheck;
        conditionToMeet = _conditionToMeet;
    }
}
