using System;
using UnityEngine;

public class StringDropdownAttribute : PropertyAttribute
{
	public string[] List { get; private set; }
	public delegate string[] GetStringList();

	public StringDropdownAttribute(params string[] list)
	{
		List = list;
	}

	public StringDropdownAttribute(Type type, string methodName)
	{
		var method = type.GetMethod(methodName);

		if (method != null)
			List = method.Invoke(null, null) as string[];

		else
			Debug.LogError("NO SUCH METHOD " + methodName + " FOR " + type);
	}
}