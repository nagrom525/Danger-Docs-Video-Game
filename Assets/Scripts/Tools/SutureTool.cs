using UnityEngine;
using System.Collections;
using System;

public class SutureTool : Tool {
    public GameObject actionButtonCanvas;
    private bool sutureNeededCalledOnce = false;

    void Start() {
        DoctorEvents.Instance.patientNeedsStitches += OnSutureNeeded;
    }

    public override ToolType GetToolType()
	{
		return ToolType.SUTURE;
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

    private void OnSutureNeeded(float duration) {
        if (!sutureNeededCalledOnce) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
            sutureNeededCalledOnce = true;
        }
    }

    void OnDestroy() {
        DoctorEvents.Instance.patientNeedsStitches -= OnSutureNeeded;
    }
}
