using UnityEngine;
using System.Collections;

public class Patient : Interactable {
	enum FlashColor 		{NORMAL, BLUE, GREEN, RED, ORANGE}
	enum StateOfAttack 		{NORMAL, ATTACKING, FINISHED}

	public float 			bpm;
    public float 			anesthetic_clock_length = 180.0f; //length of time the anesthetic clock is on in seconds

	public Transform  		hotspotSpawnPos;
	public GameObject 		scalpelTrackPrefab;
	public GameObject 		sutureHotspotsPrefab;
	public GameObject 		gauzeHotspotsPrefab;

	public GameObject[] 	toolSpawnPositions;

	private float 			last_beat_time;
	private float 			next_beat_time;
	private bool 			heart_attack;
	private float 			duration = 0.0f;
	private float 			durationOfPost = 0.0f;
	private Tool.ToolType 	requiredTool;

	// Defibulations needed to stabilize patient
	private int 			defibulationsRemaining;

	private FlashColor 		flash_color;
	private StateOfAttack 	state_attack;
	private Material 		NormalStateMaterial;
//	private float 			flash_timer = 0.0f;
//	private float 			flash_duration = 0.5f;
//	private float 			flash_time = 0.0f;

	Color 					tempColor;
	public Material 		mat;

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
		//flash_timer = duration;
		LevelUserInterface.UI.UpdateBpm (bpm);	
		this.duration = duration;
	}
		
	public void OnEnd(float duration) {
		state_attack = StateOfAttack.FINISHED;
		defibulationsRemaining = 0;
		//this.durationOfPost = duration;
		//flash_timer = duration;
	}

	public void OnGreenHeartAttack(float duration) {
		flash_color = FlashColor.GREEN;
		OnHeartAttack (duration);
		requiredTool = Tool.ToolType.TYPE_3;
		defibulationsRemaining = Random.Range(3, 6);
		print (defibulationsRemaining);
	}


	public void OnBlueHeartAttack(float duration)
	{
		flash_color = FlashColor.BLUE;
		OnHeartAttack(duration);
		requiredTool = Tool.ToolType.TYPE_1;
		defibulationsRemaining = Random.Range(3, 6);
		print(defibulationsRemaining);
	}

	//Stitches
	public void OnSuture(float duration)
	{
		Debug.Log("On suture called");
		Instantiate(sutureHotspotsPrefab, hotspotSpawnPos);
		requiredTool = Tool.ToolType.SUTURE;
	}
		
	public void OnRedHeartAttack(float duration) {
		flash_color = FlashColor.RED;
		OnHeartAttack (duration);
		requiredTool = Tool.ToolType.TYPE_2;
		defibulationsRemaining = Random.Range(3, 6);
		print (defibulationsRemaining);
	}

	public void OnOrangeHeartAttack(float duration) {
		flash_color = FlashColor.ORANGE;
		OnHeartAttack (duration);
		requiredTool = Tool.ToolType.TYPE_4;
		defibulationsRemaining = Random.Range(3, 6);
		print (defibulationsRemaining);
	}

	public void OnCutPatientOpen(float duration)
	{
		Debug.Log("on cut patient open");
		requiredTool = Tool.ToolType.SCALPEL;
	}

	// Use this for initialization
	void Start () {

		// Dummy BPM for now.
		bpm = 80f;
		LevelUserInterface.UI.UpdateBpm (bpm);
		last_beat_time = Time.time;
		DoctorEvents.Instance.onPatientCriticalEventStart += OnCutPatientOpen;
		DoctorEvents.Instance.onPatientCriticalEventEnded += OnEnd;

		DoctorEvents.Instance.patientNeedsStitches += OnSuture;

		tempColor = mat.GetColor ("_EmissionColor");
		print ("first temp color " + tempColor);
	}
	
	// Update is called once per frame
	void Update () {

		if (heart_attack || state_attack == StateOfAttack.ATTACKING) {
			if (duration <= 0.0f || state_attack == StateOfAttack.FINISHED) {
				bpm = 80f;
				LevelUserInterface.UI.UpdateBpm (bpm);
				heart_attack = false;
				state_attack = StateOfAttack.NORMAL; //take this out later
				print ("finished");
				mat.SetColor ("_EmissionColor", tempColor);
			} else {
				duration -= Time.deltaTime;
			
				
				if (flash_color == FlashColor.BLUE) {
					mat.SetColor ("_EmissionColor", Color.blue);
					//mat.color = Color.blue;
				} else if (flash_color == FlashColor.RED) {
					mat.SetColor ("_EmissionColor", Color.red);
					//mat.color = Color.red;
				} else if (flash_color == FlashColor.GREEN) {
					mat.SetColor ("_EmissionColor", Color.green);
					//mat.color = Color.green;
				} else if (flash_color == FlashColor.ORANGE) {
					mat.SetColor ("_EmissionColor", UtilityFunctions.orange);
					//mat.color = UtilityFunctions.orange;
				}
			}
		}

		/*if (state_attack == StateOfAttack.FINISHED) {
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
		}*/


		// if the time since the last heart beat has passed.
		if (Time.time > next_beat_time) {
			

			// TODO: Add heartbeat message / vitals / things here.
			// EX: renderHeartBeat();
//			print("Heartbeat Triggered.\nCurrent BPM: " + bpm);

			// update last_beat_time
			last_beat_time = Time.time;
			next_beat_time = last_beat_time + bpmToSecondsInterval(bpm);
		}
	}


	private float bpmToSecondsInterval(float bpm) {
		return (1f / (bpm / 60f));
	}


	public void receiveOperation(Tool tool, int doctorNumber = -1) {

		if (requiredTool == Tool.ToolType.SUTURE)
		{
			//Get Doctor that initiated operation
			GameObject doc = GameObject.Find("Doctor_" + doctorNumber.ToString());

			//Disable their input component

			//Create tool and give control to Doctor
	

			Debug.Log("recieving suture operation");
		}
		else
		{
			print("defibulationsRemaining: " + defibulationsRemaining);
			if (defibulationsRemaining > 0)
			{
				if (tool.GetToolType() == requiredTool)
				{
					defibulationsRemaining--;
				}
			}

			if (defibulationsRemaining == 0)
			{
				DoctorEvents.Instance.PatientCriticalAdverted();
			}		
		}

	}

	public override bool DocterIniatesInteracting(Doctor interactingDoctor)
	{
		interactingDoctor.currentTool.OnDoctorInitatedInteracting();

		Debug.Log(interactingDoctor.name + " initiated patient interaction.");

		return true;
	}


	protected override Tool.ToolType RequiredToolType()
	{
		return requiredTool;
	}
}
