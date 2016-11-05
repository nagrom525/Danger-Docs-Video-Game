using UnityEngine;
using System.Collections;

public class Doctor : MonoBehaviour {
    // CurrentTool can only be set by the Doctor when it interacts with a tool
    public Tool curentTool {get; private set;}

	// Radius of sphere for checking for interactiables.
	private float nearbyInteractableRange = 3f;

	// Use this for initialization
	void Start () {
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
		joystickVec.z = 0f;
		// Move in the direction of the joystick.
		pos += joystickVec * Time.deltaTime;
	}


	// When the interaction button is pressed, we must check to see if there
	// is and interactiable nearby. If there is, then we send a message to
	// the interactable that the doctor is initiating an interaction. The
	// interactiable accepts the interaction message and acts on it only if
	// it is valid.
	public void OnInteractionButtonPressed() {
		// May return null.
		Interactable nearbyInteractable = getNearbyInteractable(nearbyInteractableRange);

		// If there is a nearby interactable, then begin interacting!
		if (nearbyInteractable != null) {
			nearbyInteractable.DocterIniatesInteracting(this);
		}
	}

	// Gets the nearest nearby interactable within sphere of radius "range".
	private Interactable getNearbyInteractable(float range) {

		// Get the interactables. Eventually, this should take a third agrument
		// (layer mask) which ignores everything that isn't an interactable.
		Collider[] interactiablesInRange = Physics.OverlapSphere(pos, range);
		
		// Setup linear search for nearest interactable.
		Interactable nearestInteractable = null;
		float runningNearestDistance = Mathf.Infinity;

		for (int i = 0; i < interactiablesInRange.Length; i++) {
			// Get Vector3 between pos of doctor and interactable
			Vector3 interactablePos = interactiablesInRange[i].transform.position;
			// Comparing sqrDistances is faster than mag. Avoids sqrt op.
			float sqrDist = (interactablePos - pos).sqrMagnitude;
			
			// If this interactable is closer than the current closest, update
			// to this one.
			if (runningNearestDistance > sqrDist) { 
				runningNearestDistance = sqrDist;
				nearestInteractable = interactiablesInRange[i].gameObject.GetComponent<Interactable>();
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
