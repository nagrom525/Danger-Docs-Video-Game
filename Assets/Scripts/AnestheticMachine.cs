using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnestheticMachine : Interactable {


	public float anesthetic_levels = 1f;
    public GameObject actionButtonCanvas;

    private float depletion_rate;

    private bool dangerously_low_anesthetic;
    private bool canisterUsedOnce = false;
	

	private Image anestheticMeter;
	private int anestheticMeterFramesRemaining;
    bool lowAnestheticInformed = false;

	bool inTutorialState;

	void Start() {
		// 2.5% depletion per second
		depletion_rate = 0.025f;

		anestheticMeter = transform.GetComponentInChildren<Image> ();
		anestheticMeterFramesRemaining = 0;
		dangerously_low_anesthetic = false;

        DoctorEvents.Instance.onToolPickedUpCanister += OnCanisterPickedUp;
        DoctorEvents.Instance.onToolDroppedCanister += OnCanisterDropped;
		// Tutorial Startup Listeners
		TutorialEventController.Instance.OnAnestheticMachineStart += OnTutorialStateBegin;
		TutorialEventController.Instance.OnAnestheticMachienEnd += OnTutorialStateEnd;
		

		inTutorialState = false;
	}

	void Update() {

		displayAnestheticMeter();
		if (anestheticMeterFramesRemaining > 0) {
			updateAnestheticMeter ();
		} else {
			hideAnestheticMeter ();
		}

		// Drain anesthetic over time.
		if (TutorialEventController.Instance.tutorialActive)
		{
			if (inTutorialState)
			{
				// Tutorial anesthetic drain
				float pending_anesthetic_level = anesthetic_levels - depletion_rate * Time.time;
				anesthetic_levels = Mathf.Clamp(pending_anesthetic_level, 0.05f, 1f);
			}
			// Else, nothing because the machine should be stable.
		}
		else {
			drainAnesthetic();
		}

		// If we're in the tutorial, do not drain anesthetic.
        if (!inTutorialState && anesthetic_levels < 0.01f) {
            Debug.Log("anesthetic lvl == 0");
            DoctorEvents.Instance.InducePatientCritical();
        } else if (anesthetic_levels < 0.1f) {
            if (!dangerously_low_anesthetic) {
                dangerously_low_anesthetic = true;
                InvokeRepeating("flashMeter", 0f, 0.1f);
            }
        } else if (anesthetic_levels >= 0.1f) {
            dangerously_low_anesthetic = false;
            CancelInvoke("flashMeter");
        }


        // inform anesthic machine is getting low
        if(anesthetic_levels < 0.2f) {
            if (!lowAnestheticInformed) {
                DoctorEvents.Instance.InformAnestheticMachineLow(anesthetic_levels);
            }
            lowAnestheticInformed = true;
        } else if(anesthetic_levels >= 0.2f) {
            if(lowAnestheticInformed) {
                DoctorEvents.Instance.InformAnestheticMachineReturnedHigh(anesthetic_levels);
            }
            lowAnestheticInformed = false;
        }
	}

	private void flashMeter() {
		anestheticMeter.enabled = !anestheticMeter.enabled;
	}

	private void drainAnesthetic() {
		float pending_anesthetic = anesthetic_levels - depletion_rate * Time.deltaTime;
		anesthetic_levels = Mathf.Clamp(pending_anesthetic, 0f, 1f);
	}

	// The Anesthetic Machine does not require a tool to interact ... yet!
	protected override Tool.ToolType RequiredToolType() {
		return Tool.ToolType.CANISTER;
	}

	// Because the canister is consumed, the interaction does not take 
	// place over a period of time. Therefore, `interacting` should not be true.
	// Therefore, we return false.
	public override bool DocterIniatesInteracting(Doctor interactingDoctor) {
		// If doc does not have a canister, do not do anything.
		if (interactingDoctor.currentTool == null || interactingDoctor.currentTool.GetToolType() != RequiredToolType()) {
			displayAnestheticMeter ();
			return false;
		}
		// If the doctor does have a canister
		consumeCanister(interactingDoctor.currentTool as Canister);
		return false;
	}

	public void consumeCanister(Canister can) {
		float pending_anesthetic = anesthetic_levels + can.anesthetic_amount;
		anesthetic_levels = (pending_anesthetic > 1f) ? 1f : pending_anesthetic;
		Destroy (can.gameObject);
		print ("anesthetic_levels :: " + anesthetic_levels);
		displayAnestheticMeter ();
        canisterUsedOnce = true;
        actionButtonCanvas.SetActive(false);
	}


	public void displayAnestheticMeter() {
		anestheticMeter.enabled = true;
		anestheticMeterFramesRemaining = 120;
	}

	private void updateAnestheticMeter() {
		anestheticMeterFramesRemaining--;
		anestheticMeter.fillAmount = anesthetic_levels;
	}

	private void hideAnestheticMeter() {
		anestheticMeter.enabled = false;
	}

    private void OnCanisterPickedUp(float duration) {
        if(!canisterUsedOnce) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        }
    }

    private void OnCanisterDropped(float duration) {
        actionButtonCanvas.SetActive(false);
    }

	private void OnTutorialStateBegin() {
		inTutorialState = true;
		anesthetic_levels = 0.5f;
		// Drain 5% per second.
		depletion_rate = 0.05f;
	}

	private void OnTutorialStateEnd() {
		inTutorialState = false;
		// Reset variables and such
		Start();
	}

    void OnDestroy() {
        DoctorEvents.Instance.onToolPickedUpCanister -= OnCanisterPickedUp;
        DoctorEvents.Instance.onToolDroppedCanister -= OnCanisterDropped;

		TutorialEventController.Instance.OnAnestheticMachineStart -= OnTutorialStateBegin;
		TutorialEventController.Instance.OnAnestheticMachienEnd -= OnTutorialStateEnd;
    }
}
