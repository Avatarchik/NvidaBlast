using System.Collections.Generic;
using UnityEngine;

public class ExplodeAfterInstantiate : MonoBehaviour 
{
	private void OnEnable()
	{
		DestructionUpdateManager.projectileHasHitMesh += ApplyExplosionForceToFracturedMeshEVH;
	}

	private void ApplyExplosionForceToFracturedMeshEVH(Vector3 contactPoint)
	{
		var collidersInExplosionRadius = new List<Collider>();

		// This Physics.Overlap only takes in objects that are on the 'Destructable' layer mask, meaning you will 
		// have to set the object prefabs AND their fractured prefabs to 'Destructable' (allows debris to blow up)
		collidersInExplosionRadius.AddRange(Physics.OverlapSphere
			(contactPoint, CameraFiresProjectile.Singleton.fracturedExplosionRadius, LayerMask.Destructable));

		foreach (var collider in collidersInExplosionRadius)
		{
			if (collider.GetComponent<Rigidbody>() != null)
				collider.GetComponent<Rigidbody>().AddExplosionForce
					(
						CameraFiresProjectile.Singleton.initalExplosionForce, 
						contactPoint, 
						CameraFiresProjectile.Singleton.initalExplosionRadius, 
						CameraFiresProjectile.Singleton.initalUpwardsForce
					);
		}

		// Unsubscribe immediately after applying the 'explosion force' as this is only required after instantiation (you want
		// to carry the 'impact' of the projectile to the destructable mesh but it doesn't exist initally. This isn't required
		// after initalization because the impact has been carried forward via this script
		DestructionUpdateManager.projectileHasHitMesh -= ApplyExplosionForceToFracturedMeshEVH;
	}
}