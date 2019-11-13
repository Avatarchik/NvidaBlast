﻿using System.Collections;
using UnityEngine;

public class DestructionUpdateManager : MonoBehaviour 
{
	public static DestructionUpdateManager Singleton { get; private set; }

	private void Awake()
	{
		if (Singleton == null)
			Singleton = this;

		else
			Destroy(this);
	}

	public void UpdateDestruction()
	{
		StartCoroutine(UpdateDestructionCO());
	}

	public delegate void ProjectileHasHitMesh(Vector3 contactPoint);
	public static event ProjectileHasHitMesh projectileHasHitMesh;

	public IEnumerator UpdateDestructionCO()
	{
		yield return new WaitForEndOfFrame();

		if (projectileHasHitMesh != null)
			projectileHasHitMesh(contactPoint.point);
	}

	public ContactPoint contactPoint;
	public bool drawGizmos;
	public Transform destructableMeshHolder;

	private void OnDrawGizmos()
	{
		// This will return if the bool is false or if you are currently not in playmode (for some reason unity will spam
		// null reference errors for the gizmo until you enter playmode
		if (!drawGizmos || !Application.isPlaying)
			return;

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(contactPoint.point, CameraFiresProjectile.Singleton.explosionRadius);
	}
}