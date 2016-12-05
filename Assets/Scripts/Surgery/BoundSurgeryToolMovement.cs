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
		Vector3 p0Pos = p0.transform.localPosition;
		Vector3 p1Pos = p1.transform.localPosition;

		// We only need to worry about updating pos on the xz plane.
		float x = Mathf.Clamp(
			pos.x,
			p1Pos.x,
			p0Pos.x
		);

		float z = Mathf.Clamp(
			pos.z,
			p0Pos.z,
			p1Pos.z
		);

		pos = new Vector3(x, pos.y, z);
		Debug.Log("pos :: " + pos);
	}

	public Vector3 pos {
		get {
			return this.gameObject.transform.localPosition;
		}
		set {
			this.gameObject.transform.localPosition = value;
		}
	}
}
