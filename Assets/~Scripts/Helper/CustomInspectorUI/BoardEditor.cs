using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MyClass))]

public class BoardEditor : Editor
{
	private SerializedObject m_Object;
	private SerializedProperty m_Property;

	void OnEnable()
	{
		m_Object = new SerializedObject(target);
	}

	public override void OnInspectorGUI()
	{
		m_Property = m_Object.FindProperty("MyProperty");
		EditorGUILayout.PropertyField(m_Property, new GUIContent("MyLabel"), true);
		m_Object.ApplyModifiedProperties();
	}
}