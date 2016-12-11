using UnityEngine;
using System.Collections;

public class TutorialEventController : MonoBehaviour {
    enum TutorailStates { WASH_HANDS, PICK_UP_TOOL_GO_TO_PATIENT, SURGERY_ON_PATIENT, ANESTHETIC_MACHINE, HEART_ATTACK, FIRE, SCARE_AWAY_RACCON, SCARE_AWAY_BEAR}
    TutorailStates current_state;

    public bool tutorialActive;

    public delegate void WashHandsEvent(float precent, int playerNum);
    WashHandsEvent washHands;

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

    // -- Wash Hands -- //
    private void WashHandsUpdate() {

    }

    private void WashHandsComplete() {

    }

    // player Num is indexed from 0
    // want to call this function every time player washes their hands
    public void InformWashingHands(float precentWashed, int playerNum) {

    }


    // -- Pick up tool and Patient -- //
    private void PickUpToolGoToPatientUpdate() {

    }

    private void PickUpToolGoToPatientComplete() {

    }

    public void InformToolPickedUp(Tool.ToolType type, int playerNum) {

    }

    public void InformToolDropped(Tool.ToolType type, int playerNum) {

    }


    // -- Surgery On Patient -- // 
    private void SurgeryOnPatientUpdate() {

    }

    private void SurgeryOnPatientComplete() {

    }


    // -- Anesthetic Machine -- //
    public void InformSurgeryComplete(int playerNum) {

    }

    private void AnestheticMachineUpdate() {

    }


    private void InformBatteryUsed(int playerNum) {

    }

    private void AnestheticMachineComplete() {

    }


    // -- HeartAttack -- //
    private void HeartAttackUpdate() {

    }

    private void HeartAttackComplete() {

    }

    public void InformHeartAttackAdverted() {

    }


    // -- Fire -- //
    private void FireUpdate() {

    }

    private void FireComplete() {

    }

    public void InfromFirePutOut() {

    }

    // -- Scare Away Raccoon -- //
    private void ScareAwayRacconUpdate() {

    }

    private void ScareAwayRaccoonComplete() {

    }

    public void ScaredAwayRaccon(int playerNum) {

    }

    // -- Scare Away Bear -- //
    private void ScareAwayBearUpdate() {

    }

    private void ScareAwayBearComplete() {

    }

    public void InfromPlayerScaredBear(int playerNum) {

    }
}
