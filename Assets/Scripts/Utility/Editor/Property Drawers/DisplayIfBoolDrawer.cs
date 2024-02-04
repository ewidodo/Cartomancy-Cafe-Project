using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DisplayIfBoolAttribute))]
public class DisplayIfBoolDrawer : PropertyDrawer
{
    private DisplayIfBoolAttribute displayIfBool;
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
        displayIfBool = attribute as DisplayIfBoolAttribute;
        string path = property.propertyPath.Contains(".") ?
            System.IO.Path.ChangeExtension(property.propertyPath, displayIfBool.boolToCheck) : displayIfBool.boolToCheck;
        propertyToCheck = property.serializedObject.FindProperty(path);

        if (propertyToCheck == null)
        {
            // Could not find property with the specified name
            Debug.LogError($"{displayIfBool.boolToCheck} could not be found in the serialized object! Double check the variable name.");
            return false;
        }

        if (propertyToCheck.propertyType != SerializedPropertyType.Boolean)
        {
            //Not a boolean, do not draw the properties
            Debug.LogError($"{displayIfBool.boolToCheck} is not a valid boolean field.");
            return false;
        }

        return (propertyToCheck.boolValue == displayIfBool.conditionToMeet);
    }
}
