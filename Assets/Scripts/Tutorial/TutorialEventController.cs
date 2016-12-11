using UnityEngine;
using System.Collections;

public class TutorialEventController : MonoBehaviour {
    enum TutorailStates { WASH_HANDS, PICK_UP_TOOL_GO_TO_PATIENT, SURGERY_ON_PATIENT, ANESTHETIC_MACHINE, HEART_ATTACK, FIRE, SCARE_AWAY_RACCON, SCARE_AWAY_BEAR}
    TutorailStates current_state;

    private float timeStateStart = 0.0f;

	// Use this for initialization
	void Start () {
        timeStateStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        switch (current_state) {
            case TutorailStates.WASH_HANDS:
                WashHandsUpdate();
                break;
            case TutorailStates.PICK_UP_TOOL_GO_TO_PATIENT:
                PickUpToolGoToPatientUpdate();
                break;
            case TutorailStates.SURGERY_ON_PATIENT:
                SurgeryOnPatientUpdate();
                break;
            case TutorailStates.ANESTHETIC_MACHINE:
                AnestheticMachineUpdate();
                break;
            case TutorailStates.HEART_ATTACK:
                HeartAttackUpdate();
                break;
            case TutorailStates.FIRE:
                FireUpdate();
                break;
            case TutorailStates.SCARE_AWAY_RACCON:
                ScareAwayRacconUpdate();
                break;
            case TutorailStates.SCARE_AWAY_BEAR:
                ScareAwayBearUpdate();
                break;
        }
	}

    private void WashHandsUpdate() {

    }

    private void WashHandsComplete() {

    }

    private void PickUpToolGoToPatientUpdate() {

    }

    private void PickUpToolGoToPatientComplete() {

    }

    private void SurgeryOnPatientUpdate() {

    }

    private void SurgeryOnPatientComplete() {

    }

    private void AnestheticMachineUpdate() {

    }

    private void AnestheticMachineComplete() {

    }

    private void HeartAttackUpdate() {

    }

    private void HeartAttackComplete() {

    }

    private void FireUpdate() {

    }

    private void FireComplete() {

    }

    private void ScareAwayRacconUpdate() {

    }

    private void ScareAwayRaccoonComplete() {

    }

    private void ScareAwayBearUpdate() {

    }

    private void ScareAwayBearComplete() {

    }
}
