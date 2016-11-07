using UnityEngine;
using System.Collections;

public class Patient : MonoBehaviour {


	public float bpm;
    public float anesthetic_clock_length = 180.0f; //length of time the anesthetic clock is on in seconds

	private float last_beat_time;
	private float next_beat_time;
	private bool heart_attack;
	private float duration = 0.0f;

	public void OnHeartAttack (float duration) {
		heart_attack = true;
		this.duration = duration;
	}

	private static Patient _instance;
	public static Patient Instance {
		get { return _instance; }
	}

	// Use this for initialization
	void Start () {
		_instance = this;

		// Dummy BPM for now.
		bpm = 80f;

		last_beat_time = Time.time;
		DoctorEvents.Instance.heartAttackBlueEvent += OnHeartAttack;
	}
	
	// Update is called once per frame
	void Update () {

		if (heart_attack) {
			if (duration <= 0.0f) {
				bpm = 120f;
				heart_attack = false;
			} else {
				duration -= Time.deltaTime;
			}
		}


		// if the time since the last heart beat has passed.
		if (Time.time > next_beat_time) {
			

			// TODO: Add heartbeat message / vitals / things here.
			// EX: renderHeartBeat();
			print("Heartbeat Triggered.\nCurrent BPM: " + bpm);

			// update last_beat_time
			last_beat_time = Time.time;
			next_beat_time = last_beat_time + bpmToSecondsInterval(bpm);
		}
	}


	private float bpmToSecondsInterval(float bpm) {
		return (1f / (bpm / 60f));
	}


	public void receiveOperation(Tool tool) {
		switch(tool.GetToolType()) {
		case Tool.ToolType.TYPE_1:
			// React to tool 1!
		break;
		case Tool.ToolType.TYPE_2:
			// React to tool 2!
		break;
		}
	}
}
