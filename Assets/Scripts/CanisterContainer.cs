using UnityEngine;
using System.Collections;

public class CanisterContainer : MonoBehaviour {

    public GameObject actionButtonCanvas;
    private bool anestheticMachineLowCalledOnce = false;

	// Use this for initialization
	void Start () {
        DoctorEvents.Instance.onAnestheticMachineLow += OnAnestheticMachineLow;
        DoctorEvents.Instance.onAnestheticMachineReturned += OnAnestheticMachineReturned;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnAnestheticMachineLow(float precentLeft) {
        if (!anestheticMachineLowCalledOnce) {
            actionButtonCanvas.SetActive(true);
        }
        anestheticMachineLowCalledOnce = true;
    }

    private void OnAnestheticMachineReturned(float precentLeft) {
        actionButtonCanvas.SetActive(false);
    }

    void OnDestroy() {
        DoctorEvents.Instance.onAnestheticMachineLow -= OnAnestheticMachineLow;
        DoctorEvents.Instance.onAnestheticMachineReturned -= OnAnestheticMachineReturned;
    }
}
