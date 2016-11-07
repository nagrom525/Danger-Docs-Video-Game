using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class LevelUserInterface : MonoBehaviour {
    enum StatusIndicatorState { INACTIVE, BLUE_HEART_ATTACK, GREEN_HEART_ATTACK, RED_HEART_ATTACK, ORANGE_HEART_ATTACK }

	public Text heartrate;
	public Image EventSignal;
    public float statusIndicatorDuration = 5.0f;
    public float statusIndicatorBlinkDuration = 1.0f;
    public GameObject gameLostPanel;

    private StatusIndicatorState status_state = StatusIndicatorState.INACTIVE;
    private float statusIndicatorStart = 0.0f;



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

    void StatusIndicatorActiveUpdate() {

    }

    // -- Listen for events -- //
    void OnBlueHeartAttack(float duration) {
        EventSignal.material.color = Color.blue;
    }

    void OnGreenHeartAttack(float duration) {
        EventSignal.material.color = Color.green;
    }

    void OnRedHeartAttack(float duration) {
        EventSignal.material.color = Color.red;
    }

    void OnOrangeHeartAttack(float duration) {
        EventSignal.material.color = Color.ora
    }

    void OnGameOver(float duration) {
        gameLostPanel.SetActive(true);
    }

}
