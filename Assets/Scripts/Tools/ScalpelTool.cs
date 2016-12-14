using UnityEngine;
using System.Collections;
using System;

public class ScalpelTool : Tool {
    public GameObject actionButtonCanvas;
    private bool surgeryInitiated = false;
    private bool tutorialPickUpTools = false;
     
    void Start() {
        DoctorEvents.Instance.patientNeedsCutOpen += OnScalpelNeeded;
        DoctorEvents.Instance.onSurgeryOperationFirst += OnSurgeryInitiated;
        DoctorEvents.Instance.onToolDroppedForSurgery += OnScalpelDroppedForSurgery;
        TutorialEventController.Instance.OnToolDropped += OnToolDroppedTutorial;
        TutorialEventController.Instance.OnToolPickedUp += OnToolPickedUp;
        TutorialEventController.Instance.OnPickupToolsStart += OnTutorialPickUpToolsStart;
        TutorialEventController.Instance.OnPickupToolsEnd += OnTutorialPickUpToolsEnd;

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

    private void OnToolDroppedTutorial(ToolType type, int playerNum) {
        if (tutorialPickUpTools && type == ToolType.SCALPEL) {
            actionButtonCanvas.SetActive(true);
        }
    }

    private void OnToolPickedUp(ToolType type, int playerNum) {
        if(tutorialPickUpTools && type == ToolType.SCALPEL) {
            actionButtonCanvas.SetActive(false);
        }
    }

    private void OnTutorialPickUpToolsStart() {
        tutorialPickUpTools = true;
    }

    private void OnTutorialPickUpToolsEnd() {
        tutorialPickUpTools = true;
    }

    void OnDestroy() {
        DoctorEvents.Instance.patientNeedsCutOpen -= OnScalpelNeeded;
        DoctorEvents.Instance.onSurgeryOperationFirst -= OnSurgeryInitiated;
        DoctorEvents.Instance.onToolDroppedForSurgery -= OnScalpelDroppedForSurgery;
        TutorialEventController.Instance.OnToolDropped -= OnToolDroppedTutorial;
        TutorialEventController.Instance.OnToolPickedUp -= OnToolPickedUp;
        TutorialEventController.Instance.OnPickupToolsStart -= OnTutorialPickUpToolsStart;
        TutorialEventController.Instance.OnPickupToolsEnd -= OnTutorialPickUpToolsEnd;
    }

}
