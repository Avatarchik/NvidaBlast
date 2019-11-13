using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject fracturedPrefab;

	public void EnableDestructablePhysics()
	{
		// In this caes the fractured mesh isn't a good use of the object pool. We just switch the mesh out one time and we
		// don't contantly create and destroy it
		var fracturedMeshInstance = Instantiate(fracturedPrefab, transform.position, transform.rotation);
		// Set the parent to the destructableMeshHolder to keep the Hierarchy neat
		fracturedMeshInstance.transform.parent = DestructionUpdateManager.Singleton.destructableMeshHolder;
		// Remove the original non-fractured mesh
		Destroy(transform.gameObject);
	}
}
