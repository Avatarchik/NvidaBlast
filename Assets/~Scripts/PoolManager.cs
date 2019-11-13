using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	// Can you move this to it's own class later? Check if you can using SuperBallBuster
	[System.Serializable]
	public class PoolType
	{
		public string PrefabKeyName;
		public GameObject Prefab;
		public Transform ParentHolder;
		public int Size;
	}

	public static PoolManager Singleton { get; private set; }

	private void Awake()
	{
		if (Singleton == null)
			Singleton = this;

		else
			Destroy(this);
	}

	// Pool List is populated from the Unity Editor
	public List<PoolType> PoolList;
	public Dictionary<string, Queue<GameObject>> PoolDictionary;

	private void OnEnable()
	{
		CreatePool();
	}

	private void CreatePool()
	{
		PoolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach (var poolType in PoolList)
		{
			Queue<GameObject> objectPool = new Queue<GameObject>();

			for (int i = 0; i < poolType.Size; i++)
			{
				var objectInstance = Instantiate(poolType.Prefab, poolType.ParentHolder);
				objectInstance.SetActive(false);
				objectPool.Enqueue(objectInstance);
			}

			PoolDictionary.Add(poolType.PrefabKeyName, objectPool);
		}
	}

	public GameObject InstantiateFromPool(string prefabKeyName, Vector3 position, Quaternion rotation)
	{
		if (!PoolDictionary.ContainsKey(prefabKeyName))
		{
			Debug.LogWarning("Pool with tag " + prefabKeyName + " doesn't exist.");
			return null;
		}

		GameObject objectToActivate = PoolDictionary[prefabKeyName].Dequeue();

		objectToActivate.SetActive(true);
		objectToActivate.transform.position = position;
		objectToActivate.transform.rotation = rotation;

		PoolDictionary[prefabKeyName].Enqueue(objectToActivate);

		return objectToActivate;
	}

	public GameObject InstantiateFromPool(string prefabKeyName)
	{
		if (!PoolDictionary.ContainsKey(prefabKeyName))
		{
			Debug.LogWarning("Pool with tag " + prefabKeyName + " doesn't exist.");
			return null;
		}

		GameObject objectToActivate = PoolDictionary[prefabKeyName].Dequeue();

		objectToActivate.SetActive(true);

		PoolDictionary[prefabKeyName].Enqueue(objectToActivate);

		return objectToActivate;
	}

	public void ResetAndClearPool(string prefabKeyName)
	{
		if (!PoolDictionary.ContainsKey(prefabKeyName))
		{
			Debug.LogWarning("Pool with tag " + prefabKeyName + " doesn't exist.");
			return;
		}

		int size = 0;
		int activeObjects = 0;

		// Get the size of the pool that you want to reset
		foreach (var poolType in PoolList)
		{
			if (poolType.PrefabKeyName == prefabKeyName)
				size = poolType.Size;
		}

		// Get the current active objects in the pool
		foreach (var activeObject in PoolDictionary[prefabKeyName])
		{
			if (activeObject.activeInHierarchy)
				activeObjects++;
		}

		// Dequeues and deactivates any active game object in the pool
		for (var i = 0; i < size; i++)
		{
			GameObject objectToActivate = PoolDictionary[prefabKeyName].Dequeue();
			objectToActivate.SetActive(false);
			PoolDictionary[prefabKeyName].Enqueue(objectToActivate);
		}

		// Dequeues any remaning deactive game objects (i.e. unused) in the pool. This will reset the pool to the beginning
		// position (if there are six objects in the pool but you only used four, it will restart at posistion one, if you 
		// don't have this code here it would restart at position 5 [i.e. you freed up the pool, but you didn't reset the 
		// position]). This may not be required for the majority of pools, but there is no harm in resetting the position either
		for (var i = activeObjects; i < size; i++)
		{
			GameObject objectToActivate = PoolDictionary[prefabKeyName].Dequeue();
			PoolDictionary[prefabKeyName].Enqueue(objectToActivate);
		}
	}
}
