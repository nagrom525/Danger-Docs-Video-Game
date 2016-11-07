using UnityEngine;
using System.Collections;

// delegates to send events to doctors / UI
// calls delegates based on timing in levels, critical events
// informs the doctor / UI that some has occured
// main level event code
public class DoctorEvents : MonoBehaviour {
    enum HeartState { NORMAL, HEART_ATTACK, POST_HEART_ATTACK}

    // Doctors / UI elements should register with the events they care about
    public delegate void DoctorEvent();
    DoctorEvent patientCriticalEvent;
    DoctorEvent heartAttackEvent;

    // ----------- Heart Attack Values ---------------------- //
    
    public float probabiltyHeartAttack = 0.1f; // probability of heart attack per second 
    public float postHeartAttackDuration = 5.0f; // time to wait after heartattack is done before checking for another heart attack
    public float heartAttackDuration = 10.0f; // lengh of time that a heart attack occurs
    private  float heartAttackEventStartTime = 0.0f; // variable to store start time of a heart attack state
    private HeartState heartState = HeartState.NORMAL;
    private float lastTimeHeartAttackChecked = 0.0f;


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
	
	// Update is called once per frame
	void Update () {
        switch (heartState) {
            case HeartState.NORMAL:
                break;
            case HeartState.HEART_ATTACK:
                break;
            case HeartState.POST_HEART_ATTACK:
                break;
        }
	}

    //////////////////// ----------- HEART ATTACK FUNCTIONS -----------//////////////////////
    private void HeartNormalUpdate() {
        if(Time.time - lastTimeHeartAttackChecked >= 1.0f) {
            lastTimeHeartAttackChecked = Time.time;
            if(Random.value < probabiltyHeartAttack) {
                heartState = HeartState.HEART_ATTACK;
                if(heartAttackEvent != null) {
                    heartAttackEvent();
                }
                heartAttackEventStartTime = Time.time;
            }
        }

    }

    private void HeartAttackUpdate() {

    } 

    private void HeartPostAttackUpdate() {

    }

    // called when the game is supposed to end (either prematurly or due to the players running out of time due to anesthetic)
    public void EndGame() {

    }
}
