using UnityEngine;
using UnityEditor;

// ReadOnly Attribute
public class ReadOnlyAttribute : PropertyAttribute { }


public class ReadOnlyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		bool previousGUIState = GUI.enabled;
		GUI.enabled = false;
		EditorGUI.PropertyField(position, property, label, true);
		GUI.enabled = previousGUIState;
	}
}
