using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class LevelUserInterface : MonoBehaviour {
    enum StatusIndicatorState { INACTIVE, GREEN_HEART_ATTACK }

	public Text heartrate;
    public Text recipeMessage;
    public GameObject recipeMessagePanel;
	public Image statusIndicator;
    public float statusIndicatorDuration = 5.0f;
    public float statusIndicatorBlinkDuration = 1.0f;
    public GameObject gameLostPanel;

    private StatusIndicatorState status_state = StatusIndicatorState.INACTIVE;
    private float statusIndicatorStart = 0.0f;
    private float statusIndicatorLastBlinkTime = 0.0f;


    public string scalpelMessage = "Use the Scaple\n\t*Don't forget to wash you hands first!";
    public string forecepsMessage = "Use the foreceps to hold the cut open.\nSomeone else should extract the stick!";
    public string sutuersMessage = "It Looks like the patient is going to need stitches!";
    public string gauzeMessage = "It looks like the patient might need some gauze!";
    public string scuessMessage = "The suergy was a sucess good job!";


    public static LevelUserInterface UI;

	void Awake() {
		if (UI == null) {
			UI = this;
		} else {
			Debug.Log("UI only be set once");
		}
	}
	// Use this for initialization
	void Start () {
        // We probably want to register private member functions with DoctorEvents delegates
        DoctorEvents.Instance.onPatientCriticalEventStart += OnPatientCriticalEvent;
 
        DoctorEvents.Instance.GameOver += OnGameOver;
        DoctorEvents.Instance.patientNeedsCutOpen += OnCutPatientOpenEvent;
        DoctorEvents.Instance.patientDoneCutOpen += OnCutPatientOpenEnded;
        DoctorEvents.Instance.patientNeedsPullOutStick += OnPullOutStickEvent;
        DoctorEvents.Instance.patientDonePullOutStick += OnPullOutStickEnded;
        DoctorEvents.Instance.patientNeedsStitches += OnStitchesEvent;
        DoctorEvents.Instance.patientDoneStitches += OnStitchesEnded;
        DoctorEvents.Instance.patientNeedsBloodSoak += OnBloodSoakEvent;
        DoctorEvents.Instance.patientDoneBloodSoak += OnBloodSoakEnded;
	}
	
	// Update is called once per frame
	void Update () {
        if(status_state != StatusIndicatorState.INACTIVE) {
            StatusIndicatorActiveUpdate();
        }
	
	}

	public void UpdateBpm(float bpm) {
		heartrate.text = Mathf.RoundToInt(bpm).ToString () + " BPM";
		print (heartrate.text);
	}

    void StatusIndicatorActiveUpdate() {
        if((Time.time - statusIndicatorStart) > statusIndicatorDuration) {
            statusIndicator.gameObject.SetActive(false);
            statusIndicator.color = Color.white;
        } else {
            if((Time.time - statusIndicatorLastBlinkTime) > statusIndicatorBlinkDuration) {
                if (statusIndicator.gameObject.activeSelf) {
                    statusIndicator.gameObject.SetActive(false);
                } else {
                    statusIndicator.gameObject.SetActive(true);
                }
                statusIndicatorLastBlinkTime = Time.time;
            }

        }
    }

    // -- Listen for events -- //
    void OnPatientCriticalEvent(float duration) {
        statusIndicator.color = Color.green;
        statusIndicatorStart = Time.time;
        status_state = StatusIndicatorState.GREEN_HEART_ATTACK;
    }

    void OnGameOver(float duration) {
        gameLostPanel.SetActive(true);
    }

    // Game recipes

    void OnCutPatientOpenEvent(float duration) {
        recipeMessagePanel.SetActive(true);
        recipeMessage.text = scalpelMessage;
    }

    void OnCutPatientOpenEnded(float duration) {
        recipeMessagePanel.SetActive(false);
    }

    void OnPullOutStickEvent(float duration) {
        recipeMessagePanel.SetActive(true);
        recipeMessage.text = forecepsMessage;
    }

    void OnPullOutStickEnded(float duration) {
        recipeMessagePanel.SetActive(false);
    }
    
    void OnStitchesEvent(float duration) {
        recipeMessagePanel.SetActive(true);
        recipeMessage.text = sutuersMessage;
    }

    void OnStitchesEnded(float duration) {
        recipeMessagePanel.SetActive(false);
    }

    void OnBloodSoakEvent(float duration) {
        recipeMessagePanel.SetActive(true);
        recipeMessage.text = gauzeMessage;
    }

    void OnBloodSoakEnded(float duration) {
        recipeMessagePanel.SetActive(false);
    }

 

}
