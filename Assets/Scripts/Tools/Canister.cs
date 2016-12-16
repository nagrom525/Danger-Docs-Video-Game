using UnityEngine;
using System.Collections;

public class Canister : Tool {
	// A quarter tank of anesthetic
	public float anesthetic_amount = 0.35f;

    void Start() {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
	public override Tool.ToolType GetToolType() {
		return Tool.ToolType.CANISTER;
	}

	public override void OnDoctorInitatedInteracting() {
		return;
	}
	public override void OnDoctorTerminatedInteracting() {
		return;
	}


}
