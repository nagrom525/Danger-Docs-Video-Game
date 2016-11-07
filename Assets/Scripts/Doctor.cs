using UnityEngine;
using System.Collections;

public class Doctor : MonoBehaviour {
    // currentTool can only be set by the Doctor when it interacts with a tool
    public Tool currentTool {get; private set;}

	// Radius of sphere for checking for interactiables.
	private float nearbyInteractableRange = 8f;

	// Use this for initialization
	void Start () {
		currentTool = null;
	    // TODO: NEED to register event listner functions to the DoctorEvents singleton delegates
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Doctor control code (needs to be added)
    // for example... 
    public void OnJumpKeyPressed() {

    }

	// Logic for receiving joystick movement.
	// Moves the player according to the Vector3
	// recieved from input manager.
	public void OnJoystickMovement(Vector3 joystickVec) {
		// We should never be moving in the z direction.
		//joystickVec.z = 0f;
		// Move in the direction of the joystick.
		pos += joystickVec * Time.deltaTime;
	}

	public void OnPickupButtonPressed() {
		// If we currently have a tool, drop the tool.
		if (currentTool != null) {
			print ("entered if statement");
			// Add the tool back to the scene by removing parent.
			currentTool.transform.parent = null;
			// Set current tool to null.
			currentTool = null;
		} else {
			// If there is a tool in range, get that tool.
			// Otherwise, nearestTool == null
			Tool nearestTool = getNearestToolInRange(nearbyInteractableRange);

			// If there is a nearby tool, equip it.
			if (nearestTool != null) {
				// Possible Bug: Must be passed by reference? Or are game objects
				// sufficiently unique.
				equipTool (nearestTool);
			}
		}
	}

	private void equipTool(Tool tool) {
		currentTool = tool;
		tool.transform.parent = this.transform;
		// Transform tool position to doctor.
		tool.transform.localPosition = new Vector3 (1, 3, 0);
	}

	private void useCurrentToolOnPatient() {
		// if in range of patient ...
		float distToPatient = (Patient.Instance.transform.position - pos).magnitude;
		if (distToPatient <= nearbyInteractableRange) {
			// Use current tool on patient.
			Patient.Instance.receiveOperation (currentTool);
		}
	}


	// Basically the same as getNearestInteractableInRange
	private Tool getNearestToolInRange (float range) {

		// Get the interactables. Eventually, this should take a third agrument
		// (layer mask) which ignores everything that isn't an interactable.
		Collider[] toolsInRange = Physics.OverlapSphere(pos, range);

		// Setup linear search for nearest interactable.
		Tool nearestTool = null;
		float runningNearestTool = Mathf.Infinity;

		for (int i = 0; i < toolsInRange.Length; i++) {
			if (toolsInRange [i].gameObject.CompareTag ("Tool")) {
				// Get Vector3 between pos of doctor and interactable
				Vector3 toolPos = toolsInRange[i].transform.position;
				// Comparing sqrDistances is faster than mag. Avoids sqrt op.
				float sqrDist = (toolPos - pos).sqrMagnitude;

				// If this interactable is closer than the current closest, update
				// to this one.
				if (runningNearestTool > sqrDist) { 
					runningNearestTool = sqrDist;
					nearestTool = toolsInRange[i].gameObject.GetComponent<Tool>();
				}
			}
		}

		return nearestTool;
	}

	// When the interaction button is pressed, we must check to see if there
	// is and interactable nearby. If there is, then we send a message to
	// the interactable that the doctor is initiating an interaction. The
	// interactiable accepts the interaction message and acts on it only if
	// it is valid.
	public void OnInteractionButtonPressed() {
		// May return null.
		Interactable nearbyInteractable = getNearestInteractableInRange(nearbyInteractableRange);

		// If there is a nearby interactable, then begin interacting!
		if (nearbyInteractable != null) {
			nearbyInteractable.DocterIniatesInteracting(this);
		}
	}

	// Gets the nearest nearby interactable within sphere of radius "range".
	private Interactable getNearestInteractableInRange(float range) {

		// Get the interactables. Eventually, this should take a third agrument
		// (layer mask) which ignores everything that isn't an interactable.
		Collider[] interactablesInRange = Physics.OverlapSphere(pos, range);
		
		// Setup linear search for nearest interactable.
		Interactable nearestInteractable = null;
		float runningNearestDistance = Mathf.Infinity;

		for (int i = 0; i < interactablesInRange.Length; i++) {
			if (interactablesInRange[i].gameObject.CompareTag ("Interactable")) {
				// Get Vector3 between pos of doctor and interactable
				Vector3 interactablePos = interactablesInRange[i].transform.position;
				// Comparing sqrDistances is faster than mag. Avoids sqrt op.
				float sqrDist = (interactablePos - pos).sqrMagnitude;

				// If this interactable is closer than the current closest, update
				// to this one.
				if (runningNearestDistance > sqrDist) { 
					runningNearestDistance = sqrDist;
					nearestInteractable = interactablesInRange[i].gameObject.GetComponent<Interactable>();
				}
			}
		}

		return nearestInteractable;
	}

	// For my convenience
	private Vector3 pos {
		get {
			return this.transform.position;
		}
		set {
			this.transform.position = value;
		}
	}

}
