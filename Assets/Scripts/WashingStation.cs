using UnityEngine;
using System.Collections;

public class WashingStation : Interactable {

    public GameObject actionButtonCanvas;

	// Washes hands 10% at a time.
	private float washRate = 0.1f;
	private float bucketFillRate = 1f;
    private bool fireActionButtonActive = false;
    private bool washHandsActionButtonActive = false;
    private bool firePutOutOnce = false;
    private bool washedHandTutorialActive = false;
    private bool fire = false;


    void Start() {
        DoctorEvents.Instance.onDoctorNeedsToWashHands += OnDoctorNeedsToWashHands;
        DoctorEvents.Instance.onDoctorWashedHands += OnDoctorWashedHands;
        DoctorEvents.Instance.onBucketFilled += OnBucketFilled;
        DoctorEvents.Instance.onBucketEmptied += OnBucketEmptied;
        DoctorEvents.Instance.onBucketPickedUp += OnBucketPickedUp;
        DoctorEvents.Instance.onBucketDropped += OnBucketDropped;
        DoctorEvents.Instance.onFirePutOut += OnFirePutOut;
        DoctorEvents.Instance.onFire += OnFire;
        TutorialEventController.Instance.OnWashingHandsStart += OnWashHandsTutorialStart;
        TutorialEventController.Instance.OnWashingHandsEnd += OnWashHandsTutorialEnd;
    }

	// Washing station requires that you have no tool in hand.
	protected override Tool.ToolType RequiredToolType() {
		return Tool.ToolType.NONE;
	}

	public override bool DocterIniatesInteracting(Doctor interactingDoctor) {
		if (interactingDoctor.currentTool == null) {
			// If no tool in hand, wash hands.
			interactingDoctor.washHands(washRate);
            DoctorEvents.Instance.InformDoctorWashedHands();
           
			// The washing station does not require sustained interaction.
			return false;
		} else if (interactingDoctor.currentTool.GetToolType() == Tool.ToolType.BUCKET) {
			(interactingDoctor.currentTool as WaterBucket).gainWater(bucketFillRate);
            DoctorEvents.Instance.InformBucketFilled();
            if (!washHandsActionButtonActive && !washedHandTutorialActive) {
                actionButtonCanvas.SetActive(false);
            }
			return false;
		}


		Debug.Log("Cannot use this tool with WashingStation");
		return false;
	}

	public override void DoctorTerminatesInteracting(Doctor interactingDoctor) {
		interactingDoctor.currentTool.OnDoctorTerminatedInteracting();
	}

    // -- Listen for events -- //

    private void OnDoctorNeedsToWashHands(float duration) {
        if (!washedHandTutorialActive) {
            actionButtonCanvas.SetActive(true);
            washHandsActionButtonActive = true;
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        }
    }

    private void OnDoctorWashedHands(float duration) {
        if (!washedHandTutorialActive) {
            if (!washHandsActionButtonActive && !fireActionButtonActive) {
                actionButtonCanvas.SetActive(false);
            }
            washHandsActionButtonActive = false;
        }
    }

    private void OnBucketPickedUp(bool full) {
        if (!full && !firePutOutOnce) {
            if(!TutorialEventController.Instance.tutorialActive || fire) {
                fireActionButtonActive = true;
                actionButtonCanvas.SetActive(true);
                actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
            }
        }
    }

    private void OnBucketFilled(float duration) {
        if (!washHandsActionButtonActive && !washedHandTutorialActive) {
            actionButtonCanvas.SetActive(false);
            fireActionButtonActive = false;
        }
    }

    private void OnBucketEmptied(float duration) {
        if(fire) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
            fireActionButtonActive = true;
        }
    }

    private void OnBucketDropped(bool full) {
        if (!washHandsActionButtonActive && !washedHandTutorialActive) {
            actionButtonCanvas.SetActive(false);
            fireActionButtonActive = false;
        }
    }

    private void OnFire(float duration) {
        fire = true;
    }

    private void OnFirePutOut(float duration) {
        firePutOutOnce = true;
        fire = false;
    }

    private void OnWashHandsTutorialStart() {
        actionButtonCanvas.SetActive(true);
        washedHandTutorialActive = true;
        actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
    }

    private void OnWashHandsTutorialEnd() {
        actionButtonCanvas.SetActive(false);
        washedHandTutorialActive = false;
    }

    void OnDestroy() {
        DoctorEvents.Instance.onDoctorNeedsToWashHands -= OnDoctorNeedsToWashHands;
        DoctorEvents.Instance.onDoctorWashedHands -= OnDoctorWashedHands;
        DoctorEvents.Instance.onBucketPickedUp -= OnBucketPickedUp;
        DoctorEvents.Instance.onBucketDropped -= OnBucketDropped;
        DoctorEvents.Instance.onFirePutOut -= OnFirePutOut;
        DoctorEvents.Instance.onBucketFilled -= OnBucketFilled;
        DoctorEvents.Instance.onBucketEmptied -= OnBucketEmptied;
        TutorialEventController.Instance.OnWashingHandsStart -= OnWashHandsTutorialStart;
        TutorialEventController.Instance.OnWashingHandsEnd -= OnWashHandsTutorialEnd;
        DoctorEvents.Instance.onFire -= OnFire;
    }
}
