using UnityEngine;
using System.Collections;

public class WashingStation : Interactable {

    public GameObject actionButtonCanvas;

	// Washes hands 10% at a time.
	private float washRate = 0.1f;
	private float bucketFillRate = 1f;


    void Start() {
        DoctorEvents.Instance.onDoctorNeedsToWashHands += OnDoctorNeedsToWashHands;
        DoctorEvents.Instance.onDoctorWashedHands += OnDoctorWashedHands;
    }

	// Washing station requires that you have no tool in hand.
	protected override Tool.ToolType RequiredToolType() {
		return Tool.ToolType.NONE;
	}

	public override bool DocterIniatesInteracting(Doctor interactingDoctor) {
		if (interactingDoctor.currentTool == null) {
			// If no tool in hand, wash hands.
			interactingDoctor.washHands(washRate);
            DoctorEvents.Instance.InformDoctorWashedHands();
			// The washing station does not require sustained interaction.
			return false;
		} else if (interactingDoctor.currentTool.GetToolType() == Tool.ToolType.BUCKET) {
			(interactingDoctor.currentTool as WaterBucket).gainWater(bucketFillRate);
			return false;
		}


		Debug.Log("Cannot use this tool with WashingStation");
		return false;
	}

	public override void DoctorTerminatesInteracting(Doctor interactingDoctor) {
		interactingDoctor.currentTool.OnDoctorTerminatedInteracting();
	}

    private void OnDoctorNeedsToWashHands(float duration) {
        actionButtonCanvas.SetActive(true);
        actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
    }

    private void OnDoctorWashedHands(float duration) {
        actionButtonCanvas.SetActive(false);
    }
}
