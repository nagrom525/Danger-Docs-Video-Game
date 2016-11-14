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
        }
        else
		if (inputDevice.Action3.WasPressed)
        {
            //playerRenderer.material.color = Color.blue;

            //Interact
            doctor.OnInteractionButtonPressed();
			doctor.useCurrentToolOnPatient();
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

        // Rotate target object with both sticks and d-pad.

        var direction = 10.0f * new Vector3(inputDevice.Direction.X, 0, inputDevice.Direction.Y);
        //transform.Translate(direction);
        doctor.OnJoystickMovement(direction);

        transform.LookAt(transform.position + direction);
    }
}
