using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class LevelUserInterface : MonoBehaviour {
	public Text heartrate;
	public Image EventSignal;
	public float doctorBlinkDuration;
    public GameObject gameLostPanel;



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
	
	}

    // -- Listen for events -- //
    void OnBlueHeartAttack(float duration) {

    }

    void OnGreenHeartAttack(float duration) {

    }

    void OnRedHeartAttack(float duration) {

    }

    void OnOrangeHeartAttack(float duration) {

    }

    void OnGameOver(float duration) {
        gameLostPanel.SetActive(true);
    }

}
