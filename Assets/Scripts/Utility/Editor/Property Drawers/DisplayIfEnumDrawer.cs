using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(DisplayIfEnumAttribute))]
public class DisplayIfEnumDrawer : PropertyDrawer
{
    private DisplayIfEnumAttribute displayIfEnum;
    private SerializedProperty propertyToCheck;
    private float propertyHeight;
    private bool shouldDisplay;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!CheckIfDisplay(property))
        {
            return 0f;
        }
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (CheckIfDisplay(property))
        {
            EditorGUI.PropertyField(position, property);
        }
    }

    private bool CheckIfDisplay(SerializedProperty property)
    {
        displayIfEnum = attribute as DisplayIfEnumAttribute;
        string path = property.propertyPath.Contains(".") ?
            System.IO.Path.ChangeExtension(property.propertyPath, displayIfEnum.enumToCheck) : displayIfEnum.enumToCheck;
        propertyToCheck = property.serializedObject.FindProperty(path);
        // object propertyValue = propertyToCheck.GetValue();

        if (propertyToCheck == null)
        {
            // Could not find property with the specified name
            Debug.LogError($"{displayIfEnum.enumToCheck} could not be found in the serialized object! Double check the variable name.");
            return false;
        }

        if (propertyToCheck.propertyType != SerializedPropertyType.Enum)
        {
            //Not an enum, do not draw the properties
            Debug.LogError($"{displayIfEnum.enumToCheck} is not a valid enum field.");
            return false;
        }

        return (propertyToCheck.enumNames[propertyToCheck.enumValueIndex] == displayIfEnum.conditionToMeet.ToString());
    }
}
