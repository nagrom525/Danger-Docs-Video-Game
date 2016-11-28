using UnityEngine;
using System.Collections;
using System;

public class GauzeTool : Tool {
    public GameObject actionButtonCanvas;
    private bool gauzeNeededCaledOnce = false;

    void Start() {
        DoctorEvents.Instance.patientNeedsBloodSoak += OnGauzeNeeded;
    }

    public override ToolType GetToolType()
	{
		return ToolType.GAUZE;
	}

	// 
	public override void OnDoctorInitatedInteracting()
	{
        actionButtonCanvas.SetActive(false);
	}

	public override void OnDoctorTerminatedInteracting()
	{
		throw new NotImplementedException();
	}


    private void OnGauzeNeeded(float duration) {
        if (!gauzeNeededCaledOnce) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
            gauzeNeededCaledOnce = true;
        }
    }
}
