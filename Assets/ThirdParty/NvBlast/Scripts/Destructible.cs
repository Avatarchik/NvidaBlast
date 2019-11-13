using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject fracturedPrefab;

	public void EnableDestructablePhysics()
	{
		Instantiate(fracturedPrefab, transform.position, transform.rotation);
		Destroy(transform.gameObject);
	}
}
