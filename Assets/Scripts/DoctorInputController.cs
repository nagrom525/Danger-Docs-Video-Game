using UnityEngine;
using System.Collections;
using InControl;

public class DoctorInputController : MonoBehaviour
{
    public Doctor doctor;
    public int playerNum;
    Renderer playerRenderer;
    public Vector3 dir;
    
    // Use this for initialization
    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        doctor = GetComponent<Doctor>();
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
            //playerRenderer.material.color = Color.green;

            //Pickup
            doctor.OnPickupButtonPressed();
        }
        else
        if (inputDevice.Action2)
        {
			//playerRenderer.material.color = Color.red;
			doctor.Dash();
        }
        else
		if (inputDevice.Action3.WasPressed)
        {
            //playerRenderer.material.color = Color.blue;

            //Interact
            doctor.OnInteractionButtonPressed();
			//doctor.useCurrentToolOnPatient();
        }
        else
        if (inputDevice.Action4)
        {
            //playerRenderer.material.color = Color.yellow;
        }
        else
        {
            //playerRenderer.material.color = Color.white;
        }

		// If no input detected on the joysticks, set velocity to 0.
		if (Mathf.Abs(inputDevice.Direction.X) < Mathf.Epsilon && Mathf.Abs(inputDevice.Direction.Y) < Mathf.Epsilon) {
			Rigidbody rb = doctor.GetComponentInChildren<Rigidbody> ();
			rb.velocity = Vector3.zero;
		} else {
			// Rotate target object with both sticks and d-pad.
			var direction = new Vector3(inputDevice.Direction.X, 0, inputDevice.Direction.Y);
			//transform.Translate(direction);
			if (doctor.onFireFrames <= 0) {
				doctor.OnJoystickMovement(direction);
				transform.LookAt(transform.position + direction);
			}
		}
    }
}
