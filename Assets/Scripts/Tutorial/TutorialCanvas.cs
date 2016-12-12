using UnityEngine;
using System.Collections;

public class TutorialCanvas : MonoBehaviour {
    public GameObject washingHandsPanel;
    public GameObject pickUpToolPanel;
    public GameObject surgeryPanel;
    public GameObject firePanel;
    public GameObject anestheticPanel;
    public GameObject heartAttackPanel;
    public GameObject raccoonPanel;
    public GameObject bearPanel;

	// Use this for initialization
	void Start () {
        TutorialEventController.Instance.OnWashingHandsStart += OnWashHandsStart;
        TutorialEventController.Instance.OnPickupToolsStart += OnPickUpToolStart;
        TutorialEventController.Instance.OnSurgeryOnPatientStart += OnSurgeryStart;
        TutorialEventController.Instance.OnAnestheticMachineStart += OnAnestheticStart;
        TutorialEventController.Instance.OnHeartAttackStart += OnHeartAttackStart;
        TutorialEventController.Instance.OnFire += OnFireStart;
        TutorialEventController.Instance.OnScareAwayRacconStart += OnRacconStart;
        TutorialEventController.Instance.OnScareAwayBearStart += OnBearStart;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnWashHandsStart() {
        washingHandsPanel.SetActive(true);
    }

    private void OnPickUpToolStart() {
        pickUpToolPanel.SetActive(true);
    }

    private void OnSurgeryStart() {
        surgeryPanel.SetActive(true);
    }

    private void OnFireStart() {
        firePanel.SetActive(true);
    }

    private void OnAnestheticStart() {
        anestheticPanel.SetActive(true);
    }

    private void OnHeartAttackStart() {
        heartAttackPanel.SetActive(true);
    }


    private void OnRacconStart() {
        raccoonPanel.SetActive(true);
    }


    private void OnBearStart() {
        bearPanel.SetActive(true);
    }



    void OnDestroy() {
        TutorialEventController.Instance.OnWashingHandsStart -= OnWashHandsStart;
        TutorialEventController.Instance.OnPickupToolsStart -= OnPickUpToolStart;
        TutorialEventController.Instance.OnSurgeryOnPatientStart -= OnSurgeryStart;
        TutorialEventController.Instance.OnAnestheticMachineStart -= OnAnestheticStart;
        TutorialEventController.Instance.OnHeartAttackStart -= OnHeartAttackStart;
        TutorialEventController.Instance.OnFire -= OnFireStart;
        TutorialEventController.Instance.OnScareAwayRacconStart -= OnRacconStart;
        TutorialEventController.Instance.OnScareAwayBearStart -= OnBearStart;
    }

}
