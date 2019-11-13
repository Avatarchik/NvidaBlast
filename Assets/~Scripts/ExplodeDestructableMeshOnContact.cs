using System.Collections.Generic;
using UnityEngine;

public class ExplodeDestructableMeshOnContact : MonoBehaviour
{
	public List<Collider> collidersInExplosionRadius = new List<Collider>();

	private bool _runOnce;

	private void OnCollisionEnter(Collision collidedWith)
	{
		// This should only run once (OnTriggerEnter can fire multiple times causing excessive results)
		if (_runOnce)
			return;

		// Grab all the colliders in range
		collidersInExplosionRadius.AddRange(Physics.OverlapSphere
			(transform.position, CameraFiresProjectile.Singleton.explosionRadius, LayerMask.Destructable));

		// For each colider apply the explsion force to their rigidbody
		foreach (var collider in collidersInExplosionRadius)
		{
			DestructionUpdateManager.Singleton.contactPoint = collidedWith.contacts[0];

			if (collider.GetComponent<Destructible>() != null)
				collider.GetComponent<Destructible>().EnableDestructablePhysics();

			if (collider != null && collider.attachedRigidbody != null)
			{
				collider.attachedRigidbody.AddExplosionForce(CameraFiresProjectile.Singleton.explosionForce,
					DestructionUpdateManager.Singleton.contactPoint.point, CameraFiresProjectile.Singleton.explosionRadius,
					CameraFiresProjectile.Singleton.upwardsForce);
			}
		}

		_runOnce = true;
		// TODO: Object Pool
		// Set the projectile to disappear (you will have to change this out for an object pool later on)
		DestructionUpdateManager.Singleton.UpdateDestruction();
		gameObject.SetActive(false);
	}

}