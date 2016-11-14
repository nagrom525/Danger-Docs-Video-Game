using UnityEngine;
using System.Collections;
using InControl;

public class SurgeryToolInput : MonoBehaviour {
	public SurgeryTool surgeryTool;
	public int playerNum;
	Renderer playerRenderer;
	public Vector3 dir;
	public bool enableMovement = true;

	// Use this for initialization
	void Start()
	{
		playerRenderer = GetComponent<Renderer>();
		surgeryTool = GetComponent<SurgeryTool>();
	}

	void Update()
	{
		InputDevice inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices[playerNum] : null;
		if (inputDevice == null)
		{
			// If no controller exists for this doctor make it translucent.
			//playerRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
		}
		else
		{
			UpdateWithInputDevice(inputDevice);
		}
	}

	void UpdateWithInputDevice(InputDevice inputDevice)
	{
		// Set material color based on which action is pressed.
		if (inputDevice.Action1.WasPressed)
		{
			//A

			//Pickup
			surgeryTool.Activate();
		}
		else
		if (inputDevice.Action1.WasReleased)
		{
			//A
			surgeryTool.Deactivate();
		}
		else
		if (inputDevice.Action2)
		{
			//B
		}
		else
		if (inputDevice.Action3.WasPressed)
		{
			//X

			//Interact
		}
		else
		if (inputDevice.Action4)
		{
			//Y
		}
		else
		{
			//Nothig is pressed
		}

		// Rotate target object with both sticks and d-pad.

		var direction = 10.0f * new Vector3(inputDevice.Direction.X, 0, inputDevice.Direction.Y);
		//transform.Translate(direction);
		if (enableMovement)
			surgeryTool.OnJoystickMovement(direction);

	}
}
