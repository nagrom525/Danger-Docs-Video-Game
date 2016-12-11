using UnityEngine;
using System.Collections;

public class TutorialEventController : MonoBehaviour {
    enum TutorailStates { WASH_HANDS, PICK_UP_TOOL_GO_TO_PATIENT, SURGERY_ON_PATIENT, ANESTHETIC_MACHINE, HEART_ATTACK, FIRE, SCARE_AWAY_RACCON, SCARE_AWAY_BEAR, DONE}
    TutorailStates current_state;

    public bool tutorialActive { get; private set; }
    public float numPlayers = 4;

    // --            Wash Hands          -- //
    public delegate void WashHandsEvent(float precent, int playerNum);
    WashHandsEvent onWashHands;
    private float[] precentHandsWashed = new float[4];
    // -- Pick Up Tool and Go To Patient -- //
    // --       Surgery On Patient       -- //
    // --       Anesthetic Machine       -- //
    // --          Heart Attack          -- //
    // --              Fire              -- //
    // --        Scare Away Raccoon      -- //
    // --          Scare Away Bear       -- //


    private float timeStateStart = 0.0f;

    private static TutorialEventController _instance;
    public static TutorialEventController Instance {
        get { return _instance; }
    }

    void Awake() {
        // setting up singleton code
        if (_instance == null) {
            _instance = this;
        } else {
            Debug.Log("TutorialEventController can only be set once");
        }
    }

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
        bool handWashingComplete = true;
        foreach(float precent in precentHandsWashed){
            if(precent < 0.99990000) {
                handWashingComplete = false;
                break;
            }
        }
        if (handWashingComplete) {
            WashHandsComplete();
        }
    }

    private void WashHandsComplete() {
        current_state = GetNextState(current_state);
        timeStateStart = Time.time;
    }

    // player Num is indexed from 0
    // want to call this function every time player washes their hands
    public void InformWashingHands(float precentWashed, int playerNum) {
        precentHandsWashed[playerNum] = precentWashed;
        if(onWashHands != null) {
            onWashHands(precentWashed, playerNum);
        }
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

    // -- Utility -- // 
    // T is a value from 0 - 1
    // 
    private float GetT(float startTime, float totalTime) {
        return (Time.time - startTime) / totalTime;
    }

    private TutorailStates GetNextState(TutorailStates currState) {
        if(currState == TutorailStates.DONE) {
            return currState;
        } else {
            return currState + 1;
            // return (TutorailStates)(((int)currState) + 1);
        }
    }
}
