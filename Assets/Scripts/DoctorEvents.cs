using UnityEngine;
using System.Collections;

// delegates to send events to doctors / UI
// calls delegates based on timing in levels, critical events
// informs the doctor / UI that some has occured
// main level event code
public class DoctorEvents : MonoBehaviour {

    // Doctors / UI elements should register with the events they care about
    public delegate void DoctorEvent();
    DoctorEvent patientCriticalEvent;



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
        
	}

    // called when the game is supposed to end (either prematurly or due to the players running out of time due to anesthetic)
    public void EndGame() {

    }
}
