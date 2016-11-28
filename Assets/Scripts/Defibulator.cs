using UnityEngine;
using System.Collections;
using System;

public class Defibulator : Tool
{

    public GameObject actionButtonCanvas;
    private bool defibulatorNeededCalledOnce = false;

    void Start() {
        DoctorEvents.Instance.patientNeedsStitches += OnDefibulatorNeeded;
    }


    public override ToolType GetToolType()
    {
        return ToolType.DEFIBULATOR;
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

    private void OnDefibulatorNeeded(float duration) {
        if (!defibulatorNeededCalledOnce) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
            defibulatorNeededCalledOnce = true;
        }
    }
}
