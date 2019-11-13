using System.Collections;
using UnityEngine;

public class CameraFiresProjectile : MonoBehaviour 
{
	public static CameraFiresProjectile Singleton { get; private set; }

	private void Awake()
	{
		if (Singleton == null)
			Singleton = this;

		else
			Destroy(this);
	}

	public float projectileSpeed = 25f;     // Any faster than 40 and you will have problems with collision detection

	[Header("Inital Impact Settings")]  // This is required to balance the look and feel of the inital damage
	public float initalExplosionForce = 600f;
	public float initalExplosionRadius = 1.1f;
	public float initalUpwardsForce = -1f;

	[Header("Fracture Impact Settings")] 
	public float fracturedExplosionForce = 300f;
	public float fracturedExplosionRadius = 3f;
	public float fracturedUpwardsForce = 20f;

	public GameObject projectilePrefab;

	public void FireProjectile()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// If we hit something, instantiate the projectile, make it look at the hit point and acclerate it
		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			var projectileInstance = PoolManager.Singleton.InstantiateFromPool
				(projectilePrefab.name, Camera.main.transform.position, Quaternion.identity);

			projectileInstance.transform.LookAt(hit.point);

			var rigidbody = projectileInstance.GetComponent<Rigidbody>();
			rigidbody.AddForce(projectileInstance.transform.forward * rigidbody.mass * projectileSpeed * 100);

			// Set a time limit for the projectiles deactivation
			StartCoroutine(SetTimeLimitCO(projectileInstance));
		}
	}

	private IEnumerator SetTimeLimitCO(GameObject projectileInstance)
	{
		yield return new WaitForSeconds(3f);

		// If the projectile is still active then deactivate it
		if(projectileInstance.activeInHierarchy)
			projectileInstance.SetActive(false);
	}
}