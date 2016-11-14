using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Doctor : MonoBehaviour {
    // currentTool can only be set by the Doctor when it interacts with a tool
    public Tool currentTool {get; private set;}

	public float dirtLevel;
	public bool interacting;
	public bool dirtyHands {
		get { return dirtLevel > 0f; }
	}

	public Image washingMeter;
	private int washingMeterFramesRemaining;

	public Material highlightedMaterial;
	private GameObject last_interactive_obj;
	private GameObject current_interactive_obj;
	private Material original_go_material;

	// Radius of sphere for checking for interactiables.
	private float interactionRange = 8f;

	// Use this for initialization
	void Start () {
		currentTool = null;
	    // TODO: NEED to register event listner functions to the DoctorEvents singleton delegates

		// Hands start out dirty.
		dirtLevel = 1f;


		interacting = false;

		washingMeter = transform.GetComponentInChildren<Image> ();
		washingMeter.enabled = false;
		washingMeterFramesRemaining = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (washingMeterFramesRemaining > 0) {
			updateWashingMeter ();
		} else {
			hideWashingMeter ();
		}


		highlightNearestInteractiveObject ();
	}

	// Please forgive me. I tried to make this intelligable.
	private void highlightNearestInteractiveObject() {
		// Get nearest GO
		current_interactive_obj = getNearestInteractive (interactionRange);

		// If nearest object hasn't changed, there is nothing to be done
		if (current_interactive_obj == last_interactive_obj)
			return;

		// If the current object is not null ...
		if (current_interactive_obj != null) {
			// ... and the last object is not null ...
			if (last_interactive_obj != null) {
				// Then we conclude the current object is new, and switch the old object
				// back to it's original material.
				last_interactive_obj.GetComponent<Renderer> ().material = original_go_material;
			}

			// ... we highlight the current object and save its material
			Renderer rend = current_interactive_obj.GetComponent<Renderer> ();
			original_go_material = rend.material;
			rend.material = highlightedMaterial;

			// We then save current object as last object.
			last_interactive_obj = current_interactive_obj;
		}
	}

	private GameObject getNearestInteractive(float range) {
		// Get the interactables. Eventually, this should take a third agrument
		// (layer mask) which ignores everything that isn't an interactable.
		Collider[] objectsInRange = Physics.OverlapSphere(pos, range);

		// Setup linear search for nearest interactable.
		GameObject nearestObj = null;
		float runningNearestObj = Mathf.Infinity;

		for (int i = 0; i < objectsInRange.Length; i++) {
			if (objectsInRange [i].gameObject.CompareTag ("Tool")
				|| objectsInRange [i].gameObject.CompareTag ("Interactable")) {
				// Get Vector3 between pos of doctor and interactable
				Vector3 objPos = objectsInRange[i].transform.position;
				// Comparing sqrDistances is faster than mag. Avoids sqrt op.
				float sqrDist = (objPos - pos).sqrMagnitude;

				// If this interactable is closer than the current closest, update
				// to this one.
				if (runningNearestObj > sqrDist) { 
					runningNearestObj = sqrDist;
					nearestObj = objectsInRange[i].gameObject;
				}
			}
		}

		return nearestObj;
	}

	// Logic for receiving joystick movement.
	// Moves the player according to the Vector3
	// recieved from input manager.
	public void OnJoystickMovement(Vector3 joystickVec) {
		// If interacting, doctor can't move.
		if (interacting) {
			print ("currentlying interacting. Cancel interaction with action button");
			return;
		}
		// We should never be moving in the z direction.
		//joystickVec.z = 0f;
		// Move in the direction of the joystick.
		pos += joystickVec * Time.deltaTime;
	}

	public void OnPickupButtonPressed() {
		// If we currently have a tool, drop the tool.
		if (currentTool != null) {
			dropCurrentTool ();
		} else {
			// If there is a tool in range, get that tool.
			// Otherwise, nearestTool == null
			Tool nearestTool = getNearestToolInRange(interactionRange);

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
		tool.transform.localPosition = new Vector3 (1, 3, 0) * 0.5f;
		Rigidbody rb = tool.transform.GetComponent<Rigidbody> ();
		if (rb != null) {
			rb.useGravity = false;
		}
	}

	private void dropCurrentTool() {
		Rigidbody rb = currentTool.transform.GetComponent<Rigidbody> ();
		if (rb != null) {
			rb.useGravity = true;
		}
		currentTool.transform.parent = null;
		currentTool = null;
	}

	public void useCurrentToolOnPatient() {
		Debug.Log("useCurrentToolOnPatient triggered\nCurrent Tool: " + currentTool);
		// if in range of patient ...
		float distToPatient = (Patient.Instance.transform.position - pos).magnitude;
		if (distToPatient <= interactionRange) {
			// Use current tool on patient.
			Patient.Instance.receiveOperation (currentTool, GetComponent<DoctorInputController>().playerNum);
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
		// Whether you are currently interacting or not,
		// we'll want the nearest interactable.

		// May return null.
		Interactable nearbyInteractable = getNearestInteractableInRange(interactionRange);
		print ("nearbyInteractable ::" + nearbyInteractable);

		// If there is a nearby interactable, then begin interacting!
		if (nearbyInteractable != null) {
			// If currently interacting, pressing this button again will cancel the interaction.
			if (interacting) {
				nearbyInteractable.DoctorTerminatesInteracting (this);
				interacting = false;
			}
			// If not currently interacting, and ...
			// If successfully inintiated interacting, set interacting to true,
			// IF THE ACTION REQUIRES SUSTAINED INTERACTION OVER A TIME PERIOD.
			// Otherwise, false.
			interacting = nearbyInteractable.DocterIniatesInteracting (this);
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

	public void displayWashingMeter() {
		washingMeter.enabled = true;
		washingMeterFramesRemaining = 120;
	}

	private void updateWashingMeter() {
		washingMeterFramesRemaining--;
		washingMeter.fillAmount = 1f - dirtLevel;
	}

	private void hideWashingMeter() {
		washingMeter.enabled = false;
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
