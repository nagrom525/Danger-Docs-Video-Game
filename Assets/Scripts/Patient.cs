using UnityEngine;
using System.Collections;

public class Patient : MonoBehaviour {
	enum FlashColor {NORMAL, BLUE, GREEN, RED, ORANGE}
	enum StateOfAttack {NORMAL, ATTACKING, FINISHED}

	public float bpm;
    public float anesthetic_clock_length = 180.0f; //length of time the anesthetic clock is on in seconds

	private float last_beat_time;
	private float next_beat_time;
	private bool heart_attack;
	private float duration = 0.0f;
	private float durationOfPost = 0.0f;

	private FlashColor flash_color;
	private StateOfAttack state_attack;
	private Material NormalStateMaterial;
	private float flash_timer = 0.0f;
	private float flash_wait_time = 0.5f;

	private static Patient _instance;
	public static Patient Instance {
		get { return _instance; }

	}

	void Awake() {
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.Log("Patient can only be set once");
		}
	}

	public void OnHeartAttack (float duration) {
		print ("happening");
		heart_attack = true;
		state_attack = StateOfAttack.ATTACKING;
		bpm = 120f;
		LevelUserInterface.UI.UpdateBpm (bpm);	
		this.duration = duration;
	}
		
	public void OnEnd() {
		state_attack = StateOfAttack.FINISHED;
	}

	public void OnGreenHeartAttack(float duration) {
		flash_color = FlashColor.GREEN;
		OnHeartAttack (duration);
	}

	public void OnBlueHeartAttack(float duration) {
		flash_color = FlashColor.BLUE;
		OnHeartAttack (duration);
	}
		
	public void OnRedHeartAttack(float duration) {
		flash_color = FlashColor.RED;
		OnHeartAttack (duration);
	}

	public void OnOrangeHeartAttack(float duration) {
		flash_color = FlashColor.ORANGE;
		OnHeartAttack (duration);
	}

	// Use this for initialization
	void Start () {

		// Dummy BPM for now.
		bpm = 80f;
		LevelUserInterface.UI.UpdateBpm (bpm);
		last_beat_time = Time.time;
		DoctorEvents.Instance.heartAttackBlueEvent += OnBlueHeartAttack;
		DoctorEvents.Instance.heartAttackGreenEvent += OnGreenHeartAttack;
		DoctorEvents.Instance.heartAttackRedEvent += OnRedHeartAttack;
		DoctorEvents.Instance.heartAttackOrangeEvent += OnOrangeHeartAttack;

		DoctorEvents.Instance.heartAttackGreenEvent += OnEnd;
		DoctorEvents.Instance.heartAttackBlueEvent += OnEnd;
		DoctorEvents.Instance.heartAttackRedEvent += OnEnd;
		DoctorEvents.Instance.heartAttackOrangeEvent += OnEnd;

		NormalStateMaterial = GetComponent <Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {

		if (heart_attack || state_attack == StateOfAttack.ATTACKING) {
			if (duration <= 0.0f || state_attack == StateOfAttack.FINISHED) {
				bpm = 80f;
				LevelUserInterface.UI.UpdateBpm (bpm);
				heart_attack = false;
			} else {
				duration -= Time.deltaTime;
			}

			if(flash_color == FlashColor.BLUE) {
				Material temp = GetComponent <Renderer> ().material;
				temp.color = Color.blue;
				GetComponent <Renderer> ().material = temp;
				GetComponent <Renderer> ().material = NormalStateMaterial;


			} else if(flash_color == FlashColor.RED) {
				Material temp = GetComponent <Renderer> ().material;
				temp.color = Color.red;
				GetComponent <Renderer> ().material = temp;

				GetComponent <Renderer> ().material = NormalStateMaterial;

			} else if(flash_color == FlashColor.GREEN) {
				Material temp = GetComponent <Renderer> ().material;
				temp.color = Color.green;
				GetComponent <Renderer> ().material = temp;

				GetComponent <Renderer> ().material = NormalStateMaterial;

			} else if(flash_color == FlashColor.ORANGE) {

			}
		}

		if (state_attack == StateOfAttack.FINISHED) {
			if (durationOfPost <= 0.0f) {
				state_attack = StateOfAttack.NORMAL;
				GetComponent <Material>() = NormalStateMaterial;
			} else {
				durationOfPost -= Time.deltaTime;
			}

			if(flash_color == FlashColor.BLUE) {
				Material temp = GetComponent <Renderer> ().material;
				temp.color = Color.blue;
				GetComponent <Renderer> ().material = temp;
			} else if(flash_color == FlashColor.RED) {
				Material temp = GetComponent <Renderer> ().material;
				temp.color = Color.red;
				GetComponent <Renderer> ().material = temp;

			} else if(flash_color == FlashColor.GREEN) {
				Material temp = GetComponent <Renderer> ().material;
				temp.color = Color.green;
				GetComponent <Renderer> ().material = temp;

			} else if(flash_color == FlashColor.ORANGE) {

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
