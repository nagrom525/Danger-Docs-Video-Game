using UnityEngine;
using System.Collections;

public class TutorialEventController : MonoBehaviour {

    // HEARTATTACK AND FIRE // 
    // PICK_UP_TOOL_GO_TO_PATIENT,
    // SURGERY_ON_PATIENT,

    public enum TutorialStates
	{
        WELCOME,
		WASH_HANDS,
		ANESTHETIC_MACHINE,
        HEART_ATTACK,
        FIRE,
		SCARE_AWAY_RACCON,
        SCARE_AWAY_BEAR,
        PLAY_GAME,
		DONE,
		UNINITIALIZED
	}

    public delegate void PrecentPlayerNumEvent(float precent, int playerNum);
    public delegate void ToolPlayerNumEvent(Tool.ToolType type, int playerNum);
    public delegate void GeneralEvent();
    public delegate void PlayerNumEvent(int playerNum);
    TutorialStates current_state = TutorialStates.DONE;

    public bool tutorialActive = false;

    // -- Welcome -- //
    private bool[] aPressedWelcome = new bool[4];
    public PlayerNumEvent OnAPressed;
    public GeneralEvent OnWelcomeStart;
    public GeneralEvent OnWelcomeEnd;

    // -- Time to play game -- // 
    private bool[] aPressedStartGame = new bool[4];
    public GeneralEvent OnPlayGameStart;
    public GeneralEvent OnPlayGameEnd;

    // --            Wash Hands          -- //
    private float[] precentHandsWashed = new float[4];
    public PrecentPlayerNumEvent OnHandsWashed;
    public GeneralEvent OnWashingHandsStart;
    public GeneralEvent OnWashingHandsEnd;

    // -- Pick Up Tool and Go To Patient -- //
    private Tool.ToolType[] toolsHeldByDoctor = { Tool.ToolType.NONE, Tool.ToolType.NONE, Tool.ToolType.NONE, Tool.ToolType.NONE };
    private bool[] doctorAtPatient = new bool[4];
    public GeneralEvent OnPickupToolsStart;
    public GeneralEvent OnPickupToolsEnd;
    public ToolPlayerNumEvent OnToolPickedUp;
    public ToolPlayerNumEvent OnToolDropped;
    public PlayerNumEvent OnDoctorAtPatient;
    public PlayerNumEvent OnDoctorLeavesPatient;
    

    // --       Surgery On Patient       -- //
    private bool[] surgeryComplete = new bool[4];
    public GeneralEvent OnSurgeryOnPatientStart;
    public GeneralEvent OnSurgeryOnPatientEnd;
    public PlayerNumEvent OnSurgeryComplete;

    // --       Anesthetic Machine       -- //
    private bool[] batteryUsed = new bool[4];
    public GeneralEvent OnAnestheticMachineStart;
    public GeneralEvent OnAnestheticMachienEnd;
    public PlayerNumEvent OnBatteryUsed;

    // --          Heart Attack          -- //
    public GeneralEvent OnHeartAttackStart;

    // --              Fire              -- //
    public GeneralEvent OnFireStart;

    // --        Scare Away Raccoon      -- //
    private bool[] scaredAwayRaccon  = new bool[4];
    public GeneralEvent OnScareAwayRacconStart;
    public GeneralEvent OnScareAwayRacconEnd;
    public PlayerNumEvent OnPlayerScaredRaccoon;

    // --          Scare Away Bear       -- //
    private bool[] scaredAwayBear = new bool[4];
    public GeneralEvent OnScareAwayBearEnd;
    public GeneralEvent OnScareAwayBearStart;
    public PlayerNumEvent OnPlayerScaredBear;

	public GameObject PatientCameras;

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
        current_state = TutorialStates.UNINITIALIZED;
	}
	
	// Update is called once per frame
	void Update () {
        if(current_state == TutorialStates.DONE) {
            SceneTransitionController.Instance.NextScene();
        }
        switch (current_state) {
            case TutorialStates.WELCOME:
                WelcomeUpdate();
                break;
            case TutorialStates.PLAY_GAME:
                PlayGameUpdate();
                break;
            case TutorialStates.WASH_HANDS:
                WashHandsUpdate();
                break;
            //case TutorialStates.PICK_UP_TOOL_GO_TO_PATIENT:
            //    PickUpToolGoToPatientUpdate();
            //    break;
            //case TutorialStates.SURGERY_ON_PATIENT:
            //    SurgeryOnPatientUpdate();
            //    break;
            case TutorialStates.ANESTHETIC_MACHINE:
                AnestheticMachineUpdate();
                break;
            case TutorialStates.HEART_ATTACK:
                HeartAttackUpdate();
                break;
            case TutorialStates.FIRE:
                FireUpdate();
                break;
            case TutorialStates.SCARE_AWAY_RACCON:
                ScareAwayRacconUpdate();
                break;
            case TutorialStates.SCARE_AWAY_BEAR:
                ScareAwayBearUpdate();
                break;
        }
	}

    // -- Welcome -- //
    public void StartWelcome() {
        if(OnWelcomeStart != null) {
            OnWelcomeStart();
        }
    }

    public void WelcomeUpdate() {
        foreach(var aPressed in aPressedWelcome) {
            if (!aPressed) {
                return;
            }
        }
        WelcomeComplete();
    }

    public void WelcomeComplete() {
        current_state = GetNextState(current_state);
        StartNewState(current_state);
        timeStateStart = Time.time;
        if(OnWelcomeEnd != null) {
            OnWelcomeEnd();
        }
    }

    public void InformAButtonPressed(int playerNum) {
        if(current_state == TutorialStates.WELCOME) {
            aPressedWelcome[playerNum] = true;
        } else if(current_state == TutorialStates.PLAY_GAME) {
            aPressedStartGame[playerNum] = true;
        }
        if(OnAPressed != null) {
            OnAPressed(playerNum);
        }
    }

    // -- Play Game -- //

    public void StartPlayGame() {
        if(OnPlayGameStart != null) {
            OnPlayGameStart();
        }
    }

    public void PlayGameUpdate() {
        foreach(var aPressed in aPressedStartGame) {
            if (!aPressed) {
                return;
            }
        }
        PlayGameComplete();
    }

    public void PlayGameComplete() {
        current_state = GetNextState(current_state);
        StartNewState(current_state);
        timeStateStart = Time.time;
        if(OnPlayGameEnd != null) {
            OnPlayGameEnd();
        }
    }

    // -- Wash Hands -- //
    private void StartWashHands() {
        if(OnWashingHandsStart != null) {
            OnWashingHandsStart();
        }
    }

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
        StartNewState(current_state);
        timeStateStart = Time.time;
        if(OnWashingHandsEnd != null) {
            OnWashingHandsEnd();
        }
    }

    // player Num is indexed from 0
    // want to call this function every time player washes their hands
    public void InformWashingHands(float precentWashed, int playerNum) {
        if(current_state == TutorialStates.WASH_HANDS) {
            precentHandsWashed[playerNum] = precentWashed;
            if (OnHandsWashed != null) {
                OnHandsWashed(precentWashed, playerNum);
            }
        }
    }


    // -- Pick up tool and Patient -- //

    private void StartPickUpToolGoToPatient() {
        if(OnPickupToolsStart != null) {
            OnPickupToolsStart();
        }
    }

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
        StartNewState(current_state);
        if(OnPickupToolsEnd != null) {
            OnPickupToolsEnd();
        }

    }

    public void InformToolPickedUp(Tool.ToolType type, int playerNum) {
        if (tutorialActive) {
            toolsHeldByDoctor[playerNum] = type;
            OnToolPickedUp(type, playerNum);
        } 
    }

    public void InformToolDropped(Tool.ToolType type, int playerNum) {
        toolsHeldByDoctor[playerNum] = Tool.ToolType.NONE;
        OnToolDropped(type, playerNum);
    }

    public void InformDoctorAtPatient(int playerNum) {
        doctorAtPatient[playerNum] = true;
        if(OnDoctorAtPatient != null) {
            OnDoctorAtPatient(playerNum);
        }
        
    }

    public void InformDoctorLeftPatient(int playerNum) {
        doctorAtPatient[playerNum] = false;
        if(OnDoctorLeavesPatient != null) {
            OnDoctorLeavesPatient(playerNum);
        }
    }


    // -- Surgery On Patient -- // 

    private void StartSurgeryOnPatient() {
        if(OnSurgeryOnPatientStart != null) {
            OnSurgeryOnPatientStart();
        }
    }

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
        StartNewState(current_state);
        if(OnSurgeryOnPatientEnd != null) {
            OnSurgeryOnPatientEnd();
        }
    }

    public void InformSurgeryComplete(int playerNum) {
        surgeryComplete[playerNum] = true;
        if(OnSurgeryComplete != null) {
            OnSurgeryComplete(playerNum);
        }
    }



    // -- Anesthetic Machine -- //

    private void StartAnestheticMachine() {
        if(OnAnestheticMachineStart != null) {
            OnAnestheticMachineStart();
        }
    }

    private void AnestheticMachineUpdate() {
        foreach(var battery in batteryUsed) {
            if (!battery) {
                return;
            }
        }
        AnestheticMachineComplete();
    }


    public void InformBatteryUsed(int playerNum) {
        batteryUsed[playerNum] = true;
        if(OnBatteryUsed != null) {
            OnBatteryUsed(playerNum);
        }
    }

    private void AnestheticMachineComplete() {
        if(OnAnestheticMachienEnd != null) {
            OnAnestheticMachienEnd();
        }
        timeStateStart = Time.time;
        current_state = GetNextState(current_state);
        StartNewState(current_state);
    }


    // -- HeartAttack -- //
    private void StartHeartAttack() {
        if(OnHeartAttackStart != null) {
            OnHeartAttackStart();
        }
    }


    private void HeartAttackUpdate() {
       // does nothing //
    }

    private void HeartAttackComplete() {
        timeStateStart = Time.time;
        current_state = GetNextState(current_state);
        StartNewState(current_state);
    }

    public void InformHeartAttackAdverted() {
        HeartAttackComplete();
    }


    // -- Fire -- //

    private void StartFire() {
        if(OnFireStart != null) {
            OnFireStart();
        }
    }
     
    private void FireUpdate() {
        // does nothing
    }

    private void FireComplete() {
        timeStateStart = Time.time;
        current_state = GetNextState(current_state);
        StartNewState(current_state);
    }

    public void InfromFirePutOut() {
        FireComplete();
    }

    // -- Scare Away Raccoon -- //

    private void StartScareAwayRaccoon() {
        if(OnScareAwayRacconStart != null) {
            OnScareAwayRacconStart();
        }
    }

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
        StartNewState(current_state);
        if(OnScareAwayRacconEnd != null) {
            OnScareAwayRacconEnd();
        }
    }

    public void ScaredAwayRaccon(int playerNum) {
        scaredAwayRaccon[playerNum] = true;
        if(OnPlayerScaredRaccoon != null) {
            OnPlayerScaredRaccoon(playerNum);
        }
    }

    // -- Scare Away Bear -- //

    private void StartScareAwayBear() {
        if(OnScareAwayBearStart != null) {
            OnScareAwayBearStart();
        }
    }

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
        StartNewState(current_state);
        if(OnScareAwayBearEnd != null) {
            OnScareAwayBearEnd();
        }
    }

    public void InfromPlayerScaredBear(int playerNum) {
        scaredAwayBear[playerNum] = true;
        if(OnPlayerScaredBear != null) {
            OnPlayerScaredBear(playerNum);
        }
    }

    // -- Utility -- // 
    // T is a value from 0 - 1
    // 
    private float GetT(float startTime, float totalTime) {
        return (Time.time - startTime) / totalTime;
    }

    private TutorialStates GetNextState(TutorialStates currState) {
        if(currState == TutorialStates.DONE) {
            SceneTransitionController.Instance.NextScene();
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

    private void StartNewState(TutorialStates newState) {
        switch (newState) {
            case TutorialStates.WELCOME:
                StartWelcome();
                break;
            case TutorialStates.PLAY_GAME:
                StartPlayGame();
                break;
            case TutorialStates.WASH_HANDS:
                StartWashHands();
                break;
            //case TutorialStates.PICK_UP_TOOL_GO_TO_PATIENT:
            //    StartPickUpToolGoToPatient();
            //    break;
            //case TutorialStates.SURGERY_ON_PATIENT:
            //    StartSurgeryOnPatient();
            //    break;
            case TutorialStates.ANESTHETIC_MACHINE:
                StartAnestheticMachine();
                break;
            case TutorialStates.HEART_ATTACK:
                StartHeartAttack();
                break;
            case TutorialStates.FIRE:
                StartFire();
                break;
            case TutorialStates.SCARE_AWAY_RACCON:
                StartScareAwayRaccoon();
                break;
            case TutorialStates.SCARE_AWAY_BEAR:
                StartScareAwayBear();
                break;

        }
    }

    public void InformTutorialStart() {
        current_state = 0;
        StartNewState(current_state);
    }


}
