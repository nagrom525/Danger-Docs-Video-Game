using UnityEngine;
using System.Collections;
using System;

public class Defibulator : Tool
{

    public GameObject actionButtonCanvas;
    private bool defibulatorUsedOnce = false;
    public bool criticalEvent = false;

    void Start() {
        DoctorEvents.Instance.onPatientCriticalEventStart += OnDefibulatorNeeded;
        DoctorEvents.Instance.onPatientCriticalEventEnded += OnDefibulatorUsedOnce;
        DoctorEvents.Instance.onToolDroppedGeneral += OnToolDropped;
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
        if (!defibulatorUsedOnce) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
            criticalEvent = true;
        }
    }

    private void OnToolDropped(Tool.ToolType type) {
        if(type == ToolType.DEFIBULATOR) {
            if (!defibulatorUsedOnce && criticalEvent) {
                actionButtonCanvas.SetActive(true);
                actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
            }
        }
    }

    private void OnDefibulatorUsedOnce(float duration) {
        defibulatorUsedOnce = true;
        criticalEvent = false;
    }

    void OnDestroy() {
        DoctorEvents.Instance.onPatientCriticalEventStart -= OnDefibulatorNeeded;
        DoctorEvents.Instance.onPatientCriticalEventEnded -= OnDefibulatorUsedOnce;
        DoctorEvents.Instance.onToolDroppedGeneral -= OnToolDropped;
    }
}
