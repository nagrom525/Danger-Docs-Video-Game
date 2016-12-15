using UnityEngine;
using System.Collections;
using System;

public class GauzeTool : Tool {
    public GameObject actionButtonCanvas;
    private bool gauzeNeededCaledOnce = false;
    private bool tutorialPickUpTools = false;
    private bool pickedUpForTutorial = false;


    void Start() {
        DoctorEvents.Instance.patientNeedsBloodSoak += OnGauzeNeeded;
        TutorialEventController.Instance.OnToolDropped += OnToolDroppedTutorial;
        TutorialEventController.Instance.OnToolPickedUp += OnToolPickedUpTutorial;
        TutorialEventController.Instance.OnPickupToolsStart += OnTutorialPickUpToolsStart;
        TutorialEventController.Instance.OnPickupToolsEnd += OnTutorialPickUpToolsEnd;
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

    private void OnToolDroppedTutorial(ToolType type, int playerNum) {
        if (tutorialPickUpTools && type == ToolType.GAUZE) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        } else if (type == ToolType.GAUZE) {
            pickedUpForTutorial = false;
        }
    }

    private void OnToolPickedUpTutorial(ToolType type, int playerNum) {
        if (tutorialPickUpTools && type == ToolType.GAUZE) {
            actionButtonCanvas.SetActive(false);
        } else if (type == ToolType.GAUZE) {
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
        tutorialPickUpTools = true;
        actionButtonCanvas.SetActive(false);
    }

    void OnDestroy() {
        DoctorEvents.Instance.patientNeedsBloodSoak -= OnGauzeNeeded;
        TutorialEventController.Instance.OnToolDropped -= OnToolDroppedTutorial;
        TutorialEventController.Instance.OnToolPickedUp -= OnToolPickedUpTutorial;
        TutorialEventController.Instance.OnPickupToolsStart -= OnTutorialPickUpToolsStart;
        TutorialEventController.Instance.OnPickupToolsEnd -= OnTutorialPickUpToolsEnd;
    }
}
