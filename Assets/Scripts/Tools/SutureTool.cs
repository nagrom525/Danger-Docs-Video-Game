using UnityEngine;
using System.Collections;
using System;

public class SutureTool : Tool {
    public GameObject actionButtonCanvas;
    private bool sutureNeededCalledOnce = false;
    private bool tutorialPickUpTools = false;
    private bool pickedUpForTutorial = false;

    void Start() {
        DoctorEvents.Instance.patientNeedsStitches += OnSutureNeeded;
        TutorialEventController.Instance.OnToolDropped += OnToolDroppedTutorial;
        TutorialEventController.Instance.OnToolPickedUp += OnToolPickedUpTutorial;
        TutorialEventController.Instance.OnPickupToolsStart += OnTutorialPickUpToolsStart;
        TutorialEventController.Instance.OnPickupToolsEnd += OnTutorialPickUpToolsEnd;
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

    private void OnToolDroppedTutorial(ToolType type, int playerNum) {
        if (tutorialPickUpTools && type == ToolType.SUTURE) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        } else if (type == ToolType.SUTURE) {
            pickedUpForTutorial = false;
        }
    }

    private void OnToolPickedUpTutorial(ToolType type, int playerNum) {
        if (tutorialPickUpTools && type == ToolType.SUTURE) {
            actionButtonCanvas.SetActive(false);
        } else if (type == ToolType.SUTURE) {
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
        DoctorEvents.Instance.patientNeedsStitches -= OnSutureNeeded;
        TutorialEventController.Instance.OnToolDropped -= OnToolDroppedTutorial;
        TutorialEventController.Instance.OnToolPickedUp -= OnToolPickedUpTutorial;
        TutorialEventController.Instance.OnPickupToolsStart -= OnTutorialPickUpToolsStart;
        TutorialEventController.Instance.OnPickupToolsEnd -= OnTutorialPickUpToolsEnd;
    }
}
