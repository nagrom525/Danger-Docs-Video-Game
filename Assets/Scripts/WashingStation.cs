using UnityEngine;
using System.Collections;

public class WashingStation : Interactable {

	// Washes hands 10% at a time.
	private float washRate = 0.1f;
	private float bucketFillRate = 1f;

	// Washing station requires that you have no tool in hand.
	protected override Tool.ToolType RequiredToolType() {
		return Tool.ToolType.NONE;
	}

	public override bool DocterIniatesInteracting(Doctor interactingDoctor) {
		if (interactingDoctor.currentTool == null) {
			// If no tool in hand, wash hands.
			washHands(interactingDoctor);
			print ("Washing Station Activated");
			// The washing station does not require sustained interaction.
			return false;
		} else if (interactingDoctor.currentTool.GetToolType() == Tool.ToolType.BUCKET) {
			(interactingDoctor.currentTool as WaterBucket).gainWater(bucketFillRate);
			return false;
		}


		Debug.Log("Cannot use this tool with WashingStation");
		return false;
	}

	private void washHands(Doctor interactingDoctor) {
		interactingDoctor.dirtLevel -= washRate;
		if (interactingDoctor.dirtLevel <= 0f) {
			interactingDoctor.dirtLevel = 0f;
		}
		interactingDoctor.displayWashingMeter ();
		print ("dirtLevel ::" + interactingDoctor.dirtLevel);
	}

	public override void DoctorTerminatesInteracting(Doctor interactingDoctor) {
		interactingDoctor.currentTool.OnDoctorTerminatedInteracting();
	}
}
