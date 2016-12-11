using UnityEngine;
using System.Collections;

public class TutorialEventController : MonoBehaviour {
    enum TutorailStates { WASH_HANDS, PICK_UP_TOOL_GO_TO_PATIENT, SURGERY_ON_PATIENT, ANESTHETIC_MACHINE, HEART_ATTACK, FIRE, SCARE_AWAY_RACCON, SCARE_AWAY_BEAR, DONE}
    TutorailStates current_state;

    public bool tutorialActive { get; private set; }


    // --            Wash Hands          -- //
    public delegate void WashHandsEvent(float precent, int playerNum);
    WashHandsEvent onWashHands;
    private float[] precentHandsWashed = new float[4];

    // -- Pick Up Tool and Go To Patient -- //
    private Tool.ToolType[] toolsHeldByDoctor = { Tool.ToolType.NONE, Tool.ToolType.NONE, Tool.ToolType.NONE, Tool.ToolType.NONE };
    private bool[] doctorAtPatient = new bool[4];

    // --       Surgery On Patient       -- //
    private bool[] surgeryComplete = new bool[4];

    // --       Anesthetic Machine       -- //
    private bool[] batteryUsed = new bool[4];
    public delegate void GeneralEvent();
    public GeneralEvent OnAnestheticMachineStart;
    public GeneralEvent OnAnestheticMachienEnd;

    // --          Heart Attack          -- //

    // --              Fire              -- //

    // --        Scare Away Raccoon      -- //
    private bool[] scaredAwayRaccon  = new bool[4];

    // --          Scare Away Bear       -- //
    private bool[] scaredAwayBear = new bool[4];


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
        foreach(float precent in precentHandsWashed){
            if(precent < 0.99990000) {
                return;
            }
        }
        WashHandsComplete();
    }

    private void WashHandsComplete() {
        current_state = GetNextState(current_state);
        timeStateStart = Time.time;
    }

    // player Num is indexed from 0
    // want to call this function every time player washes their hands
    public void InformWashingHands(float precentWashed, int playerNum) {
        if(current_state == TutorailStates.WASH_HANDS) {
            precentHandsWashed[playerNum] = precentWashed;
            if (onWashHands != null) {
                onWashHands(precentWashed, playerNum);
            }
        }
    }


    // -- Pick up tool and Patient -- //
    private void PickUpToolGoToPatientUpdate() {
        foreach(var tool in toolsHeldByDoctor) {
            if(tool == Tool.ToolType.NONE) {
                return;
            }
        }
        foreach(bool atPatient in doctorAtPatient) {
            if (!atPatient) {
                return;
            }
        }
        PickUpToolGoToPatientComplete();
    }

    private void PickUpToolGoToPatientComplete() {
        current_state = GetNextState(current_state);
        timeStateStart = Time.time;
        
    }

    public void InformToolPickedUp(Tool.ToolType type, int playerNum) {
            toolsHeldByDoctor[playerNum] = type;
    }

    public void InformToolDropped(Tool.ToolType type, int playerNum) {
        toolsHeldByDoctor[playerNum] = Tool.ToolType.NONE;
    }

    public void InformDoctorAtPatient(int playerNum) {
        doctorAtPatient[playerNum] = true;
    }

    public void InformDoctorLeftPatient(int playerNum) {
        doctorAtPatient[playerNum] = false;
    }


    // -- Surgery On Patient -- // 
    private void SurgeryOnPatientUpdate() {
        foreach(var complete in surgeryComplete) {
            if (!complete) {
                return;
            }
        }
        SurgeryOnPatientComplete();
    }

    private void SurgeryOnPatientComplete() {
        timeStateStart = Time.time;
        current_state = GetNextState(current_state);
    }

    public void InformSurgeryComplete(int playerNum) {
        surgeryComplete[playerNum] = true;
    }



    // -- Anesthetic Machine -- //
    private void AnestheticMachineUpdate() {
        foreach(var battery in batteryUsed) {
            if (!battery) {
                return;
            }
        }
        AnestheticMachineComplete();
    }


    private void InformBatteryUsed(int playerNum) {
        batteryUsed[playerNum] = true;
    }

    private void AnestheticMachineComplete() {
        timeStateStart = Time.time;
        current_state = GetNextState(current_state);
    }


    // -- HeartAttack -- //
    private void HeartAttackUpdate() {
       // does nothing //
    }

    private void HeartAttackComplete() {
        timeStateStart = Time.time;
        current_state = GetNextState(current_state);
    }

    public void InformHeartAttackAdverted() {
        HeartAttackComplete();
    }


    // -- Fire -- //
    private void FireUpdate() {
        // does nothing
    }

    private void FireComplete() {
        timeStateStart = Time.time;
        current_state = GetNextState(current_state);
    }

    public void InfromFirePutOut() {
        FireComplete();
    }

    // -- Scare Away Raccoon -- //
    private void ScareAwayRacconUpdate() {
        foreach(var scaredAway in scaredAwayRaccon) {
            if (!scaredAway) {
                return;
            }
        }
        ScareAwayBearComplete();
    }

    private void ScareAwayRaccoonComplete() {
        timeStateStart = Time.time;
        current_state = GetNextState(current_state);
    }

    public void ScaredAwayRaccon(int playerNum) {
        scaredAwayRaccon[playerNum] = true;
    }

    // -- Scare Away Bear -- //
    private void ScareAwayBearUpdate() {
        foreach(var scaredAway in scaredAwayBear) {
            if (!scaredAway) {
                return;
            }
        }
        ScareAwayBearComplete();
    }

    private void ScareAwayBearComplete() {
        timeStateStart = Time.time;
        current_state = GetNextState(current_state);
    }

    public void InfromPlayerScaredBear(int playerNum) {
        scaredAwayBear[playerNum] = true;
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

    private void resetBools(bool[] boolArray) {
        for(int i = 0; i < boolArray.Length; ++i) {
            boolArray[i] = false;
        }
    }


}
