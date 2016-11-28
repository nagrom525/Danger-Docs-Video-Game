using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class LevelUserInterface : MonoBehaviour {
    enum StatusIndicatorState { INACTIVE, GREEN_HEART_ATTACK }

	public Text heartrate;
    public Text recipeMessage;
    public GameObject recipeMessagePanel;
    public GameObject gameWonPanel;
	public Image statusIndicator;
    public float statusIndicatorDuration = 5.0f;
    public float statusIndicatorBlinkDuration = 1.0f;
    public GameObject gameLostPanel;

    private StatusIndicatorState status_state = StatusIndicatorState.INACTIVE;
    private float statusIndicatorStart = 0.0f;
    private float statusIndicatorLastBlinkTime = 0.0f;



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
		DoctorEvents.Instance.GameWon += OnGameWon;
	}
	
	// Update is called once per frame
	void Update () {
        if(status_state != StatusIndicatorState.INACTIVE) {
            StatusIndicatorActiveUpdate();
        }
	
	}

	public void UpdateBpm(float bpm) {
		heartrate.text = Mathf.RoundToInt(bpm).ToString () + " BPM";
		//print (heartrate.text);
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

    void OnGameWon(float duration) {
        gameWonPanel.SetActive(true);
    }


 

}
