using UnityEngine;
using System.Collections;
using System;

public class ScalpelTool : Tool {
    public GameObject actionButtonCanvas;
    private bool surgeryInitiated = false;
     
    void Start() {
        DoctorEvents.Instance.patientNeedsCutOpen += OnScalpelNeeded;
        DoctorEvents.Instance.onSurgeryOperationFirst += OnSurgeryInitiated;
        DoctorEvents.Instance.onToolDroppedForSurgery += OnScalpelDroppedForSurgery;
    }


	public override ToolType GetToolType()
	{
		return ToolType.SCALPEL;
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

    private void OnScalpelNeeded(float duration) {
        if (!surgeryInitiated) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        }
    }


    private void OnSurgeryInitiated(float duration) {
        surgeryInitiated = true;
    }

    private void OnScalpelDroppedForSurgery(Tool.ToolType type) {
        if((type == ToolType.SCALPEL) && !surgeryInitiated) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        }
    }

}
