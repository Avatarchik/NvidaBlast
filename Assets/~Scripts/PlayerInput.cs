using UnityEngine;

public class PlayerInput : MonoBehaviour 
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(MouseButton.LeftMouse))
			CameraFiresProjectile.Singleton.FireProjectile();
	}
}