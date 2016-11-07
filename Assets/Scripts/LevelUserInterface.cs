using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using UnityEditor;

public class LevelUserInterface : MonoBehaviour {
    enum StatusIndicatorState { INACTIVE, BLUE_HEART_ATTACK, GREEN_HEART_ATTACK, RED_HEART_ATTACK, ORANGE_HEART_ATTACK }

	public Text heartrate;
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
        DoctorEvents.Instance.heartAttackGreenEvent += OnGreenHeartAttack;
        DoctorEvents.Instance.heartAttackBlueEvent += OnBlueHeartAttack;
        DoctorEvents.Instance.heartAttackRedEvent += OnRedHeartAttack;
        DoctorEvents.Instance.heartAttackOrangeEvent += OnOrangeHeartAttack;
        DoctorEvents.Instance.GameOver += OnGameOver;
	}
	
	// Update is called once per frame
	void Update () {
        if(status_state != StatusIndicatorState.INACTIVE) {
            StatusIndicatorActiveUpdate();
        }
	
	}

	public void UpdateBpm(float bpm) {
		heartrate.text = bpm.ToString () + " BPM";
	}

    void StatusIndicatorActiveUpdate() {
        if((Time.time - statusIndicatorStart) > statusIndicatorDuration) {
            statusIndicator.gameObject.SetActive(false);
        } else {
            if((Time.time - statusIndicatorLastBlinkTime) > statusIndicatorBlinkDuration) {
                if (statusIndicator.gameObject.activeSelf) {
                    statusIndicator.gameObject.SetActive(false);
                } else {
                    statusIndicator.gameObject.SetActive(true);
                }
            }

        }
    }

    // -- Listen for events -- //
    void OnBlueHeartAttack(float duration) {
        statusIndicator.material.color = Color.blue;
        statusIndicatorStart = Time.time;
    }

    void OnGreenHeartAttack(float duration) {
        statusIndicator.material.color = Color.green;
        statusIndicatorStart = Time.time;
    }

    void OnRedHeartAttack(float duration) {
        statusIndicator.material.color = Color.red;
        statusIndicatorStart = Time.time;
    }

    void OnOrangeHeartAttack(float duration) {
        statusIndicator.material.color = UtilityFunctions.orange;
        statusIndicatorStart = Time.time;
    }

    void OnGameOver(float duration) {
        gameLostPanel.SetActive(true);
    }

}
