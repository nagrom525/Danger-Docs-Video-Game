using UnityEngine;
using System.Collections;

public class BoundSurgeryToolMovement : MonoBehaviour {

	// Assuming p0 is the bottom left corner
	// and p1 is the top right corner.

	public GameObject p0;
	public GameObject p1;

	void Start() {
		p0 = Patient.Instance.transform.Find("bound0").gameObject;
		p1 = Patient.Instance.transform.Find("bound1").gameObject;
	}

	void Update() {

		// We only need to worry about updating pos on the xz plane.
		float x = Mathf.Clamp(pos.x, p0.transform.position.x, p1.transform.position.x);
		float z = Mathf.Clamp(pos.z, p0.transform.position.z, p1.transform.position.z);

		pos = new Vector3(x, pos.y, z);
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
