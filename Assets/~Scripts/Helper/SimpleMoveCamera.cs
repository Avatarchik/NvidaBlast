using UnityEngine;

public class SimpleMoveCamera : MonoBehaviour
{
	public float lookSpeed = 60f;
	public float zoomSpeed = 8f;
	public float panSpeed = 35f;
	public float walkSpeed = 18f;

	private Vector2 _rotation = new Vector2(0, 0);

	private void Start()
	{
		_rotation.x = transform.eulerAngles.x;
		_rotation.y = transform.eulerAngles.y;
	}

	void Update()
	{
		transform.Translate(GetKeyboardInput());

		// Look Around
		if (Input.GetMouseButton(MouseButton.RightMouse))
		{
			_rotation.y += Input.GetAxis("Mouse X") * Time.deltaTime * lookSpeed;
			_rotation.x -= Input.GetAxis("Mouse Y") * Time.deltaTime * lookSpeed;

			transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
		}

		// Pan Camera
		if (Input.GetMouseButton(MouseButton.MiddleMouse))
			transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * panSpeed,
				-Input.GetAxisRaw("Mouse Y") * Time.deltaTime * panSpeed, 0);

		// Zoom using the MouseWheel
		transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.Self);
	}

	private Vector3 GetKeyboardInput()
	{
		Vector3 keyMovement = new Vector3();

		// Move forward/back
		if (Input.GetKey(KeyCode.W))
			keyMovement += new Vector3(0, 0, 1) * Time.deltaTime * walkSpeed;

		if (Input.GetKey(KeyCode.S))
			keyMovement += new Vector3(0, 0, -1) * Time.deltaTime * walkSpeed;

		// Strafe Left/Right
		if (Input.GetKey(KeyCode.A))
			keyMovement += new Vector3(-1, 0, 0) * Time.deltaTime * walkSpeed;

		if (Input.GetKey(KeyCode.D))
			keyMovement += new Vector3(1, 0, 0) * Time.deltaTime * walkSpeed;

		// Move up/down
		if (Input.GetKey(KeyCode.Q))
			keyMovement += new Vector3(0, 1, 0) * Time.deltaTime * walkSpeed;

		if (Input.GetKey(KeyCode.E))
			keyMovement += new Vector3(0, -1, 0) * Time.deltaTime * walkSpeed;

		return keyMovement;
	}
}