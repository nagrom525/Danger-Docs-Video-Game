using UnityEngine;
using System.Collections;

public class BoundSurgeryToolMovement : MonoBehaviour {

	public Bounds bounds;
	public Vector3 boundsCenterOffset;

	void Awake() {
		Camera patientCamera = Patient.Instance.GetComponentInChildren<Camera> ();
		Utils.SetCameraBounds (patientCamera);
		// Then set them
		bounds = Utils.CombineBoundsOfChildren(this.gameObject);
		// Also find the diff between bounds.center & transform.position.
		boundsCenterOffset = bounds.center - pos;
	}
		
	void Update () {
		// Check if tool is out of bounds and correct position.
		// Calculate offset
		Vector3 offset = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
		if (offset != Vector3.zero) {
			print ("offset :: " + offset);
			// Add offset to current position to prevent moving out of bounds.
//			pos += offset;
		}

		// Update bounds
		bounds.center = pos + boundsCenterOffset;
	}

	public Vector3 pos {
		get {
			return this.gameObject.transform.position;
		}
		set {
			this.gameObject.transform.position = value;
		}
	}
}
