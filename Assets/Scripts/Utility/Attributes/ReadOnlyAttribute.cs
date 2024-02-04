using UnityEngine;
using System;

/// <summary>
/// Disallows editing of property/fields inside the Unity Editor
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class ReadOnlyAttribute : PropertyAttribute
{
    
}
