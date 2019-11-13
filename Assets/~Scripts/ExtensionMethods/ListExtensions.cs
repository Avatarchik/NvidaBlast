using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListExtensions
{
	public static T GetRandom<T>(this IList<T> list)
	{
		return list[Random.Range(0, list.Count)];
	}

	public static T GetRandomAndRemove<T>(this IList<T> list)
	{
		var randomItem = list[Random.Range(0, list.Count)];
		list.Remove(randomItem);

		return randomItem;
	}

	public static List<T> ShuffleList<T>(this List<T> list)
	{
		list = list
			.OrderBy(listItem => Random.value)
			.ToList();

		return list;
	}

	// This probably should be done with a HashSet (doesn't allow duplicates) but since Unity doesn't work well with Hash Sets
	// in the inspector we can do it this way instead
	public static bool AddUnique<T>(this List<T> list, T item)
	{
		if (list.Contains(item))
			return true;

		else
		{
			list.Add(item);
			return false;
		}
	}

	public static void FindValueAndReplace<T>(this List<T> list, T oldValue, T newValue)
	{
		var index = list.IndexOf(oldValue);

		if (index != -1)
			list[index] = newValue;

		else
			list.Add(newValue);
	}

	public static List<T> SortDescending<T>(this List<T> list)
	{
		list = list
			.OrderByDescending(item => item)
			.ToList();

		return list;
	}

	public static List<T> SortAscending<T>(this List<T> list)
	{
		list = list
			.OrderBy(item => item)
			.ToList();

		return list;
	}
}
