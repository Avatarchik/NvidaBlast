using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject fracturedPrefab;

	public void EnableDestructablePhysics()
	{
		var fracturedMeshInstance = Instantiate(fracturedPrefab, transform.position, transform.rotation);
		fracturedMeshInstance.transform.parent = DestructionUpdateManager.Singleton.destructableMeshHolder;
		Destroy(transform.gameObject);
	}
}
