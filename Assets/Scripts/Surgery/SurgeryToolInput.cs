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

	public void ReturnControlToDoctor()
	{
        if (TutorialEventController.Instance.tutorialActive) {
            TutorialEventController.Instance.OnDoctorLeavesPatient(playerNum);
        }
		
		//Get Doctor that initiated operation
		GameObject doc = GameObject.Find("Doctor_" + (playerNum + 1).ToString());
		if (doc == null)
		{
			Debug.Log("couldn't find doctor!");
		}
		//Disable their input component
		doc.GetComponent<DoctorInputController>().enabled = true;
        doc.GetComponent<Doctor>().informSurgeryFinished();
        DoctorEvents.Instance.InformDoctorLeftSurgeryOperaton();

		if (this.gameObject != null) {
			Destroy(this.gameObject);
		}
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
		Quaternion patient_cam_rotation = Patient.Instance.GetComponentInChildren<Camera>().transform.rotation;
		direction = patient_cam_rotation * direction;
		// HACK: This is to make the rotation correct dispite movement.
		surgeryTool.transform.localRotation = new Quaternion(0f, 270f, 0f, 0f);
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
