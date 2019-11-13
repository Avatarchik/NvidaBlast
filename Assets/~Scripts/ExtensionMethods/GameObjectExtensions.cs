using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
	public static void ApplyToChildren(this GameObject gameObject, Action<GameObject> action)
	{
		if (gameObject == null) return;
		foreach (var trans in gameObject.GetComponentsInChildren<Transform>(true))
		{
			action(trans.gameObject);
		}
	}

	public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag)
			where T : Component
	{
		// http://www.third-helix.com/2013/09/30/adding-to-unitys-builtin-classes-using-extension-methods.html
		List<T> results = new List<T>();

		if (gameObject.CompareTag(tag))
			results.Add(gameObject.GetComponent<T>());

		foreach (Transform t in gameObject.transform)
			results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));

		// Another piece of code which uses Linq that may also work if you can get it working
		//https://answers.unity.com/questions/893966/how-to-find-child-with-tag.html
		//GetComponentsInChildren().Where(r => r.tag == "Your Tag").ToArray()[0];

		return results.ToArray();
	}

	public static T GetComponentInChildrenWithTag<T>(this GameObject gameObject, string tag)
			where T : Component
	{
		var singleComponent = gameObject.GetComponentsInChildren<T>(true).Where
			(
				gameObjectWithSpecifiedComponentT => gameObjectWithSpecifiedComponentT.tag == tag
			)
			.ToArray()[0];

		return singleComponent;
	}

	public static T GetComponentInParentWithTag<T>(this GameObject gameObject, string tag)
		where T : Component
	{
		var singleComponent = gameObject.GetComponentsInParent<T>(true).Where
			(
				gameObjectWithSpecifiedComponentT => gameObjectWithSpecifiedComponentT.tag == tag
			)
			.ToArray()[0];

		return singleComponent;
	}
}