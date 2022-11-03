using UnityEditor;
using UnityEngine;

namespace TemporaryGameCompany
{
    [CustomPropertyDrawer(typeof(FloatReference))]
    public class ReferenceDrawer : PropertyDrawer
    {
        private readonly string[] popupOptions = {"User Constant", "Use Variable"}; // Options of drawer button.
        private GUIStyle popupStyle; // Style of drawer button.

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Style of drawer.
            popupStyle = GUI.skin.GetStyle("PaneOptions"); // Sets popup style.
            popupStyle.imagePosition = ImagePosition.ImageOnly; // Only displays image of GUI.

            label = EditorGUI.BeginProperty(position, label, property); // Label marks beginning of property.
            position = EditorGUI.PrefixLabel(position, label); // Label is a prefix to property.

            EditorGUI.BeginChangeCheck(); // Checks for changed.

            // Get Properties of Refrerence.
            SerializedProperty useConstant = property.FindPropertyRelative("UseConstant");
            SerializedProperty constantValue = property.FindPropertyRelative("ConstantValue");
            SerializedProperty variable = property.FindPropertyRelative("Variable");

            // Calculate rect for configuration button.
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += popupStyle.margin.top;
            buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            int indent = EditorGUI.indentLevel; // Stores old indent level.
            EditorGUI.indentLevel = 0; // Sets indent level to 0.

            int result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, popupOptions, popupStyle); // Gets result of user's choice.

            useConstant.boolValue = result == 0; // Sets value based on user's choice (inversed).

            EditorGUI.PropertyField(position, useConstant.boolValue ? constantValue : variable, GUIContent.none); // Displays reference value.

            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties(); // Changes value in reference if needed.

            EditorGUI.indentLevel = indent; // Resets Indent.
            EditorGUI.EndProperty(); // End of property.


        }
    }
}