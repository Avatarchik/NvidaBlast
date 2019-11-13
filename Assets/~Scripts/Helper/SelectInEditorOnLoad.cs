using UnityEditor;
using UnityEngine;

public class SelectInEditorOnLoad : MonoBehaviour
{
	public GameObject gameObjectToSelect;
    // Start is called before the first frame update
    void Start()
    {
		EditorGUIUtility.PingObject(gameObjectToSelect);
		Selection.activeGameObject = gameObjectToSelect;
	}
}
