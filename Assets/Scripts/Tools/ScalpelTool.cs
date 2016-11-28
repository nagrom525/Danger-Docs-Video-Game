using UnityEngine;
using System.Collections;
using System;

public class ScalpelTool : Tool {
    public GameObject actionButtonCanvas;
    private bool scalpelNeededCalledOnce = false;
     
    void Start() {
        DoctorEvents.Instance.patientNeedsCutOpen += OnScalpelNeeded;
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
        if (!scalpelNeededCalledOnce) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
            scalpelNeededCalledOnce = true;
        }
    }

}
