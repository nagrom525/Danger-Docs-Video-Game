using UnityEngine;
using System.Collections;
using System;

public class ScalpelTool : Tool {
    public GameObject actionButtonCanvas;
    private bool surgeryInitiated = false;
    private bool tutorialPickUpTools = false;
    private bool pickedUpForTutorial = false;
     
    void Start() {
        DoctorEvents.Instance.patientNeedsCutOpen += OnScalpelNeeded;
        DoctorEvents.Instance.onSurgeryOperationFirst += OnSurgeryInitiated;
        DoctorEvents.Instance.onToolDroppedForSurgery += OnScalpelDroppedForSurgery;
        TutorialEventController.Instance.OnToolDropped += OnToolDroppedTutorial;
        TutorialEventController.Instance.OnToolPickedUp += OnToolPickedUpTutorial;
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
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        } else if (type == ToolType.SCALPEL) {
            pickedUpForTutorial = false;
        }
    }

    private void OnToolPickedUpTutorial(ToolType type, int playerNum) {
        if(tutorialPickUpTools && type == ToolType.SCALPEL) {
            actionButtonCanvas.SetActive(false);
        } else if(type == ToolType.SCALPEL) {
            pickedUpForTutorial = true;
        }
    }

    private void OnTutorialPickUpToolsStart() {
        tutorialPickUpTools = true;
        if (!pickedUpForTutorial) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        }
    }

    private void OnTutorialPickUpToolsEnd() {
        tutorialPickUpTools = false;
        actionButtonCanvas.SetActive(false);
    }

    void OnDestroy() {
        DoctorEvents.Instance.patientNeedsCutOpen -= OnScalpelNeeded;
        DoctorEvents.Instance.onSurgeryOperationFirst -= OnSurgeryInitiated;
        DoctorEvents.Instance.onToolDroppedForSurgery -= OnScalpelDroppedForSurgery;
        TutorialEventController.Instance.OnToolDropped -= OnToolDroppedTutorial;
        TutorialEventController.Instance.OnToolPickedUp -= OnToolPickedUpTutorial;
        TutorialEventController.Instance.OnPickupToolsStart -= OnTutorialPickUpToolsStart;
        TutorialEventController.Instance.OnPickupToolsEnd -= OnTutorialPickUpToolsEnd;
    }

}
