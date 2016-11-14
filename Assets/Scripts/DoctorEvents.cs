using UnityEngine;
using System.Collections;

// delegates to send events to doctors / UI
// calls delegates based on timing in levels, critical events
// informs the doctor / UI that some has occured
// main level event code
public class DoctorEvents : MonoBehaviour {
    // States handlers ///
    public enum MainReciepeState { CUT_OPEN, POST_CUT_OPEN, PULL_OUT_STICK, POST_PULL_OUT, SOAK_BLOOD, POST_SOAK_BLOOD, STICH_BODY, POST_STITCH_BODY }
    public enum GeneralGameState { NORMAL, PATIENT_CRITICAL, POST_PATIENT_CRITICAL, GAME_OVER}
    public static MainReciepeState[] scene1ReciepeElements = new MainReciepeState[4] { MainReciepeState.CUT_OPEN, MainReciepeState.PULL_OUT_STICK, MainReciepeState.SOAK_BLOOD, MainReciepeState.STICH_BODY };
    public GeneralGameState gameState = GeneralGameState.NORMAL;
    private int currentIndexInReciepe = 0;

    // Doctors / UI elements should register with the events they care about
    public delegate void DoctorEvent(float duration);

    // Cut open patient event
    public DoctorEvent patientNeedsCutOpen;
    public DoctorEvent patientDoneCutOpen;

    // Pull out stick event
    public DoctorEvent patientNeedsPullOutStick;
    public DoctorEvent patientDonePullOutStick;

    // Soak blood event
    public DoctorEvent patientNeedsBloodSoak;
    public DoctorEvent patientDoneBloodSoak;

    // Stitch Patient
    public DoctorEvent patientNeedsStitches;
    public DoctorEvent patientDoneStitches;

    // -- Patient Critical Events -- //
    public DoctorEvent onPatientCriticalEventStart;
    public DoctorEvent onPatientCriticalEventEnded;

    // -- Game over event -- //
    public DoctorEvent GameOver;


    // --- Main Reciepe Values --//
    public float timeDelayPostCutOpen = 2.0f;
    public float timeDelayPostStickPullOut = 2.0f;
    public float timeDealyPostSoakBlood = 2.0f;
    public float timeDelayPostStiches = 2.0f;



    // ----------- Heart Attack Values ---------------------- //
    // ONE THING WE CAN DO: is start the heart attack duration at a base time, and add some kind of randomness to the rest of the time.
    //                      That way we have a base time that the heart rate would last, but the duration of the heart attack might still seem a bit random

    public float probabiltyPatientCritical = 0.05f; // probability of heart attack per second 
    public float postPatientCriticalDuration = 5.0f; // time to wait after heartattack is done before checking for another heart attack
    public float patientCriticalDuration = 10.0f; // lengh of time that a heart attack occurs
    private  float patientCriticalStartTime = 0.0f; // variable to store start time of a heart attack state
    private float lastTimePatientCriticalChecked = 0.0f;
    private Tool.ToolType activeTool = Tool.ToolType.NONE;


    private static DoctorEvents _instance;
    public static DoctorEvents Instance {
        get { return _instance; }
    }

    void Awake() {
        // setting up singleton code
        if (_instance == null) {
            _instance = this;
        } else {
            Debug.Log("DoctorEvents can only be set once");
        }
    }

	// Use this for initialization
	void Start () {
      
	}

    void Update() {
        switch (gameState) {
            case GeneralGameState.NORMAL:
                GameNormalUpdate();
                break;
            case GeneralGameState.PATIENT_CRITICAL:
                GamePatientCriticalUpdate();
                break;
        }
    }
	
    private void GameNormalUpdate() {
        if ((Time.time - lastTimePatientCriticalChecked) >= 1.0f) {
            lastTimePatientCriticalChecked = Time.time;

            if (Random.value < probabiltyPatientCritical) {
                gameState = GeneralGameState.PATIENT_CRITICAL;
                patientCriticalStartTime = Time.time;
                if (onPatientCriticalEventStart != null) {
                    onPatientCriticalEventStart(patientCriticalDuration);
                }
            }
        }
    }

    private void GamePatientCriticalUpdate() {
        if ((Time.time - patientCriticalStartTime) > patientCriticalDuration) {
            EndPatientCritical();
            OnPatientDeath();
        }
    }


    private void HeartPostAttackUpdate() {
        if ((Time.time - patientCriticalStartTime) > patientCriticalDuration) {
            gameState = gameState = GeneralGameState.NORMAL;
            lastTimePatientCriticalChecked = Time.time;
            patientCriticalStartTime = Time.time;
        }

    }


    private void EndPatientCritical() {
        gameState = GeneralGameState.POST_PATIENT_CRITICAL;
        patientCriticalStartTime = Time.time;
        if(onPatientCriticalEventEnded != null) {
            onPatientCriticalEventEnded(postPatientCriticalDuration){
            }
        }

    }

    // called when the game is supposed to end (either prematurly or due to the players running out of time due to anesthetic)
    // gives the player one more chance by sending the patient into critical
    public void OnPatientCritical() {
        if(gameState != GeneralGameState.PATIENT_CRITICAL) {
            gameState = GeneralGameState.PATIENT_CRITICAL;
            if (onPatientCriticalEventStart != null) {
                onPatientCriticalEventStart(patientCriticalDuration);
            }
        }
    }

    public void PatientCriticalAdverted() {
        if (gameState == GeneralGameState.PATIENT_CRITICAL) {
            EndPatientCritical();
        } else {
            Debug.Log("Heart Attack adverted when the patient wasn't in a Heart Attack");
        }
    }

    // actually ends the game 
    // if the player critical state isn't adverted
    public void OnPatientDeath() {
        Time.timeScale = 0.0f;
        if (GameOver != null) {
            GameOver(0.0f);
        }
    }

    //////         ----------- Main Event Public Interface -----------              ///////////////
    public void OnPatientCutOpen() {
        if(patientDoneCutOpen != null) {
            patientDoneCutOpen(0);
        }
    }

    public void OnPatientStickPulledOut() {
        if (patientDonePullOutStick != null) {
            patientDonePullOutStick(0);
        }
    }

    public void OnPatientBloodSoaked() {
        if (patientDoneBloodSoak != null) {
            patientDoneBloodSoak(0);
        }
    }

    public void OnPatientStitched() {
        if(patientDoneStitches != null) {
            patientDoneStitches(0);
        }
    }
}
