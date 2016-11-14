using UnityEngine;
using System.Collections;

// delegates to send events to doctors / UI
// calls delegates based on timing in levels, critical events
// informs the doctor / UI that some has occured
// main level event code
public class DoctorEvents : MonoBehaviour {
    // States handlers ///
    public enum MainReciepeState { CUT_OPEN, POST_CUT_OPEN, PULL_OUT_STICK, POST_PULL_OUT, SOAK_BLOOD, POST_SOAK_BLOOD, STICH_BODY, POST_STITCH_BODY }
    public static MainReciepeState[] scene1ReciepeElements = new MainReciepeState[4] { MainReciepeState.CUT_OPEN, MainReciepeState.PULL_OUT_STICK, MainReciepeState.SOAK_BLOOD, MainReciepeState.STICH_BODY };
    private int currentIndexInReciepe = 0;

    // Doctors / UI elements should register with the events they care about
    public delegate void DoctorEvent(float duration);
    public DoctorEvent patientCriticalEvent;

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



    // -- Heart Attack Blue Delegates -- //
    public DoctorEvent heartAttackBlueEvent;
    public DoctorEvent heartAttackBlueEnded;

    // -- Heart Attack Green Delegates -- //
    public DoctorEvent heartAttackGreenEvent;
    public DoctorEvent heartAttackGreenEnded;

    // -- Heart Attack Red Delegates -- //
    public DoctorEvent heartAttackRedEvent;
    public DoctorEvent heartAttackRedEnded;

    // -- Heart Attack Orange Delegates -- //
    public DoctorEvent heartAttackOrangeEvent;
    public DoctorEvent heartAttackOrangeEnded;

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

    //public float probabiltyHeartAttack = 0.1f; // probability of heart attack per second 
    //public float postHeartAttackDuration = 5.0f; // time to wait after heartattack is done before checking for another heart attack
    //public float heartAttackDuration = 10.0f; // lengh of time that a heart attack occurs
    //private  float heartEventStartTime = 0.0f; // variable to store start time of a heart attack state
    ////private RecipeState heartState = RecipeState.NORMAL;
    //private float lastTimeHeartAttackChecked = 0.0f;
    //private Tool.ToolType activeTool = Tool.ToolType.NONE;


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
	
	//// Update is called once per frame
	//void Update () {
 //       switch (heartState) {
 //           case RecipeState.NORMAL:
 //               HeartNormalUpdate();
 //               break;
 //           case RecipeState.HEART_ATTACK:
 //               HeartAttackUpdate();
 //               break;
 //           case RecipeState.POST_HEART_ATTACK:
 //               HeartPostAttackUpdate();
 //               break;
 //       }
	//}

    ////////////////////// ----------- HEART ATTACK FUNCTIONS -----------//////////////////////
    //private void HeartNormalUpdate() {
    //    if((Time.time - lastTimeHeartAttackChecked) >= 1.0f) {
    //        lastTimeHeartAttackChecked = Time.time;

    //        if(Random.value < probabiltyHeartAttack) {
    //            heartState = RecipeState.HEART_ATTACK;
    //            // figure out with tool is required to send out the appropiate event
    //            int randomTool = Random.Range(0, 3);
    //            switch (randomTool) {
    //                case 0:
    //                    activeTool = Tool.ToolType.TYPE_1;
    //                    if(heartAttackBlueEvent != null) {
    //                        heartAttackBlueEvent(heartAttackDuration);
    //                    }
    //                    break;
    //                case 1:
    //                    activeTool = Tool.ToolType.TYPE_2;
    //                    if (heartAttackGreenEvent != null) {
    //                        heartAttackGreenEvent(heartAttackDuration);
    //                    }
    //                    break;
    //                case 2:
    //                    activeTool = Tool.ToolType.TYPE_3;
    //                    if (heartAttackRedEvent != null) {
    //                        heartAttackRedEvent(heartAttackDuration);
    //                    }
    //                    break;
    //                case 3:
    //                    activeTool = Tool.ToolType.TYPE_4;
    //                    if (heartAttackOrangeEvent != null) {
    //                        heartAttackOrangeEvent(heartAttackDuration);
    //                    }
    //                    break;
    //            }
    //            heartEventStartTime = Time.time;
    //        }
    //    }

    //}

    //private void HeartAttackUpdate() {
    //    if((Time.time - heartEventStartTime) > heartAttackDuration) {
    //        EndHeartAttack();
    //        OnPatientDeath();
    //    }
    //} 

    //private void HeartPostAttackUpdate() {
    //    if((Time.time - heartEventStartTime) > postHeartAttackDuration) {
    //        heartState = RecipeState.NORMAL;
    //        lastTimeHeartAttackChecked = Time.time;
    //        heartEventStartTime = Time.time;
    //    }

    //}

    //private void EndHeartAttack() {
    //    heartState = RecipeState.POST_HEART_ATTACK;
    //    heartEventStartTime = Time.time;
    //    switch (activeTool) {
    //        case Tool.ToolType.TYPE_1:
    //            if (heartAttackBlueEnded != null) {
    //                heartAttackBlueEnded(postHeartAttackDuration);
    //            }
    //            break;
    //        case Tool.ToolType.TYPE_2:
    //            if (heartAttackGreenEnded != null) {
    //                heartAttackGreenEnded(postHeartAttackDuration);
    //            }
    //            break;
    //        case Tool.ToolType.TYPE_3:
    //            if (heartAttackRedEnded != null) {
    //                heartAttackRedEnded(postHeartAttackDuration);
    //            }
    //            break;
    //        case Tool.ToolType.TYPE_4:
    //            if (heartAttackOrangeEnded != null) {
    //                heartAttackOrangeEnded(postHeartAttackDuration);
    //            }
    //            break;
    //    }
    //}

    //// called by external function when the heart attack has been adverted
    //public void HeartAttackAdverted() {
    //    if(heartState == RecipeState.HEART_ATTACK) {
    //        EndHeartAttack();
    //    }else {
    //        Debug.Log("Heart Attack adverted when the patient wasn't in a Heart Attack");
    //    }
    //}

    // called when the game is supposed to end (either prematurly or due to the players running out of time due to anesthetic)
    public void OnPatientDeath() {
        Time.timeScale = 0.0f;
        if(GameOver != null) {
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
