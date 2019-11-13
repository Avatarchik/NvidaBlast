using System.Collections.Generic;
using UnityEngine;

public class ExplodeDestructableMeshOnContact : MonoBehaviour
{
	private void OnDisable()
	{
		_runOnce = false;

		GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	private bool _runOnce;

	private void OnCollisionEnter(Collision collidedWith)
	{
		// This should only run once (OnTriggerEnter can fire multiple times causing excessive results)
		if (_runOnce)
			return;

		// Grab all the colliders in range
		var collidersInExplosionRadius = new List<Collider>();

		collidersInExplosionRadius.AddRange(Physics.OverlapSphere
			(transform.position, CameraFiresProjectile.Singleton.explosionRadius, LayerMask.Destructable));

		// For each colider apply the explsion force to their rigidbody
		foreach (var collider in collidersInExplosionRadius)
		{
			// Save out the point of contact
			DestructionUpdateManager.Singleton.contact = collidedWith.contacts[0];

			if (collider.GetComponent<Destructible>() != null)
				collider.GetComponent<Destructible>().EnableDestructablePhysics();

			// This code will effect the debris lying on the ground, it doesn't effect the fractured mesh that will be
			// instantiated in the next frame
			if (collider != null && collider.attachedRigidbody != null)
			{
				collider.attachedRigidbody.AddExplosionForce(CameraFiresProjectile.Singleton.explosionForce,
					DestructionUpdateManager.Singleton.contact.point, CameraFiresProjectile.Singleton.explosionRadius,
					CameraFiresProjectile.Singleton.upwardsForce);
			}
		}

		_runOnce = true;
		DestructionUpdateManager.Singleton.UpdateDestruction();

		// Set the projectile to false because it is used in an object pool
		gameObject.SetActive(false);
	}

}