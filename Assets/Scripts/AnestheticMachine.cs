using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnestheticMachine : Interactable {


	public float anesthetic_levels = 1f;
    public GameObject actionButtonCanvas;

    private float depletion_rate;

    private bool dangerously_low_anesthetic;
	private bool patient_critical;
    private bool canisterUsedOnce = false;
	

	private Image anestheticMeter;
	private int anestheticMeterFramesRemaining;
    bool lowAnestheticInformed = false;

	void Start() {
		// 5% depletion per second
		depletion_rate = 0.025f;

		anestheticMeter = transform.GetComponentInChildren<Image> ();
		anestheticMeterFramesRemaining = 0;
		dangerously_low_anesthetic = false;
		patient_critical = false;

		DoctorEvents.Instance.onPatientCriticalEventEnded += onPatientCriticalEventAverted;
        DoctorEvents.Instance.onToolPickedUpCanister += OnCanisterPickedUp;
        DoctorEvents.Instance.onToolDroppedCanister += OnCanisterDropped;
	}

	void Update() {
		displayAnestheticMeter();
		if (anestheticMeterFramesRemaining > 0) {
			updateAnestheticMeter ();
		} else {
			hideAnestheticMeter ();
		}

		// Drain anesthetic over time.
		drainAnesthetic();

        if (anesthetic_levels < 0.01f) {
            if (!patient_critical) {
                patient_critical = true;
                Debug.Log("anesthetic lvl == 0");
                DoctorEvents.Instance.InducePatientCritical();
            }
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

	private void onPatientCriticalEventAverted(float duration) {
		patient_critical = false;
	}

	private void flashMeter() {
		anestheticMeter.enabled = !anestheticMeter.enabled;
	}

	private void drainAnesthetic() {
		float pending_anesthetic = anesthetic_levels - depletion_rate * Time.deltaTime;
		anesthetic_levels = (pending_anesthetic < 0f) ? 0f : pending_anesthetic;
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

}
