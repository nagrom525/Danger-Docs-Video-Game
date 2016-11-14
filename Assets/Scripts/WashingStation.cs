using UnityEngine;
using System.Collections;

public class WashingStation : Interactable {

	// Washes hands 10% at a time.
	private float washRate = 0.1f;

	// Washing station requires that you have no tool in hand.
	protected override Tool.ToolType RequiredToolType() {
		return Tool.ToolType.NONE;
	}

	public override bool DocterIniatesInteracting(Doctor interactingDoctor) {
		if (interactingDoctor.currentTool != null) {
			// Doctor has tool => Cannot wash hands until puts down tool.
			print("Tool in hand -> cannot use Washing Station");
			return false;
		}
		// If no tool in hand, wash hands.
		washHands(interactingDoctor);
		print ("Washing Station Activated");
		// The washing station does not require sustained interaction.
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
