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
        DoctorEvents.Instance.onBearStealsPatient += OnBearStealsPatient;
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

	void ReturnControlToDoctor()
	{
		//Get Doctor that initiated operation
		GameObject doc = GameObject.Find("Doctor_" + (playerNum + 1).ToString());
		if (doc == null)
		{
			Debug.Log("couldn't find doctor!");
		}
		//Disable their input component
		doc.GetComponent<DoctorInputController>().enabled = true;
        DoctorEvents.Instance.InformDoctorLeftSurgeryOperaton();
		doc.GetComponent<Doctor>().inSurgery = false;
		Destroy(this.gameObject);
	}

	void UpdateWithInputDevice(InputDevice inputDevice)
	{
		// Set material color based on which action is pressed.
		if (inputDevice.Action1.WasPressed)
		{
			//A
		}
		else
		if (inputDevice.Action1.WasReleased)
		{
			//A
		}
		else
		if (inputDevice.Action2)
		{
			//B
			ReturnControlToDoctor();
		}
		else
		if (inputDevice.Action3.WasPressed)
		{
			//X

			//Interact
			surgeryTool.Activate();
		}
        else
		if (inputDevice.Action3.WasReleased)
		{
			//X
			surgeryTool.Deactivate();
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
		direction = surgeryTool.transform.rotation * direction;
		//transform.Translate(direction);
		if (enableMovement)
		{
			surgeryTool.OnJoystickMovement(direction);
		}
	}

    public void OnBearStealsPatient(float duration) {
        ReturnControlToDoctor();
    }
}
