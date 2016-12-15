using UnityEngine;
using System.Collections;

public class CanisterContainer : MonoBehaviour {

    public GameObject actionButtonCanvas;
    private bool anestheticMachineLowCalledOnce = false;
    int canisterPickedUpCount = 0;
    bool inTutorialState = false;

	// Use this for initialization
	void Start () {
        DoctorEvents.Instance.onAnestheticMachineLow += OnAnestheticMachineLow;
        DoctorEvents.Instance.onAnestheticMachineReturned += OnAnestheticMachineReturned;
        TutorialEventController.Instance.OnAnestheticMachineStart += OnAnestheticMachineTutorialStart;
        TutorialEventController.Instance.OnAnestheticMachienEnd += OnAnestheticMachineTutorialEnd;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnAnestheticMachineLow(float precentLeft) {
        if (!anestheticMachineLowCalledOnce && !inTutorialState) {
            actionButtonCanvas.SetActive(true);
        }
        anestheticMachineLowCalledOnce = true;
    }

    private void OnAnestheticMachineReturned(float precentLeft) {
        if (!inTutorialState) {
            actionButtonCanvas.SetActive(false);
        }
    }

    private void OnAnestheticMachineTutorialStart() {
        actionButtonCanvas.SetActive(true);
        actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
    }

    private void OnAnestheticMachineTutorialEnd() {
        actionButtonCanvas.SetActive(false);
    }

    void OnDestroy() {
        DoctorEvents.Instance.onAnestheticMachineLow -= OnAnestheticMachineLow;
        DoctorEvents.Instance.onAnestheticMachineReturned -= OnAnestheticMachineReturned;
        TutorialEventController.Instance.OnAnestheticMachineStart -= OnAnestheticMachineTutorialStart;
        TutorialEventController.Instance.OnAnestheticMachienEnd -= OnAnestheticMachineTutorialEnd;
    }
}
