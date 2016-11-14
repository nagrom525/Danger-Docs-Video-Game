using UnityEngine;
using System.Collections;

public class Patient : MonoBehaviour {
	enum FlashColor {NORMAL, BLUE, GREEN, RED, ORANGE}
	enum PatientCriticalState {NORMAL, SPEEDING_UP, ATTACKING, FINISHING}

    public float anesthetic_clock_length = 180.0f; //length of time the anesthetic clock is on in seconds

	public Transform  	hotspotSpawnPos;
	public GameObject 	scalpelTrackPrefab;
	public GameObject 	sutureHotspotsPrefab;
	public GameObject 	gauzeHotspotsPrefab;

	private float last_beat_time;
	private float next_beat_time;
	private bool inCriticalState;
	private float criticalStateDuration = 0.0f;
	private float critalStatePostDuration = 0.0f;
	private Tool.ToolType requiredTool;

	// Defibulations needed to stabilize patient
	private int defibulationsRemaining;


    // --- Heart rate and patient critical state -- //
    public float normal_bpm = 80.0f;
    public float critical_bpm = 200.0f;
    public float hear_rate_modulation_range = 20.0f;

    private FlashColor flash_color;
	private PatientCriticalState critical_state;
	private Material NormalStateMaterial;
    private float timeStartCriticalState;
    private float timeMidleCriticalState;
    private float timeEndCriticalState;
    private float bpm;
    //	private float flash_timer = 0.0f;
    //	private float flash_duration = 0.5f;
    //	private float flash_time = 0.0f;

    Color tempColor;
	public Material mat;

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

	public void OnPatientCriticalEventStart (float duration) {
		print ("happening");
		inCriticalState = true;
		critical_state = PatientCriticalState.ATTACKING;
		bpm = 120f;
		//flash_timer = duration;
		LevelUserInterface.UI.UpdateBpm (bpm);	
		this.criticalStateDuration = duration;
	}

    public void OnPatientCriticalEventEnded(float duration) {
        critical_state = PatientCriticalState.FINISHING;
        defibulationsRemaining = 0;
    }

	//Stitches
	public void OnSuture(float duration)
	{
		Debug.Log("On suture called");
		Instantiate(sutureHotspotsPrefab, hotspotSpawnPos);
		requiredTool = Tool.ToolType.SUTURE;
	}
		

	public void OnCutPatientOpen(float duration)
	{
		Debug.Log("oncutpatientopen");
		requiredTool = Tool.ToolType.SCALPEL;
	}

	// Use this for initialization
	void Start () {
        bpm = normal_bpm;
		LevelUserInterface.UI.UpdateBpm (bpm);
		last_beat_time = Time.time;
        next_beat_time = last_beat_time + bpmToSecondsInterval(bpm);
        DoctorEvents.Instance.onPatientCriticalEventStart += OnPatientCriticalEventStart;
		DoctorEvents.Instance.onPatientCriticalEventEnded += OnPatientCriticalEventEnded;
	

		DoctorEvents.Instance.patientNeedsStitches += OnSuture;
		tempColor = mat.GetColor ("_EmissionColor");
		print ("first temp color " + tempColor);
	}

    // Update is called once per frame
    void Update() {
        if (inCriticalState || critical_state == PatientCriticalState.ATTACKING) {
            if (criticalStateDuration <= 0.0f || critical_state == PatientCriticalState.FINISHING) {
                inCriticalState = false;
                critical_state = PatientCriticalState.NORMAL; //take this out later
                print("finished");
                mat.SetColor("_EmissionColor", tempColor);
            } else {
                criticalStateDuration -= Time.deltaTime;


                if (flash_color == FlashColor.BLUE) {
                    mat.SetColor("_EmissionColor", Color.blue);
                    //mat.color = Color.blue;
                } else if (flash_color == FlashColor.RED) {
                    mat.SetColor("_EmissionColor", Color.red);
                    //mat.color = Color.red;
                } else if (flash_color == FlashColor.GREEN) {
                    mat.SetColor("_EmissionColor", Color.green);
                    //mat.color = Color.green;
                } else if (flash_color == FlashColor.ORANGE) {
                    mat.SetColor("_EmissionColor", UtilityFunctions.orange);
                    //mat.color = UtilityFunctions.orange;
                }
            }
        }
        // if the time since the last heart beat has passed.
        if (Time.time > next_beat_time) {
            // update last_beat_time
            last_beat_time = Time.time;
            next_beat_time = last_beat_time + bpmToSecondsInterval(bpm);
            // randomly increment or decrement heart rate within window (add a modulation to the heart rate)
            if(0.333333f < Random.value) {
                if (bpm > critical_bpm - 20) {
                    --bpm;
                    LevelUserInterface.UI.UpdateBpm(bpm);
                }
            } else if(0.666666f < Random.value) {
              if(bpm < critical_bpm + 20) {
                    ++bpm;
                    LevelUserInterface.UI.UpdateBpm(bpm);
                }
            }

        }


        // TODO: Add heartbeat message / vitals / things here.
        // EX: renderHeartBeat();
        //			print("Heartbeat Triggered.\nCurrent BPM: " + bpm);

    }


	private float bpmToSecondsInterval(float bpm) {
		return (1f / (bpm / 60f));
	}


	public void receiveOperation(Tool tool) {
		print ("defibulationsRemaining: " + defibulationsRemaining);
		if (defibulationsRemaining > 0) {
			if (tool.GetToolType() == requiredTool) {
				defibulationsRemaining--;
			}
		}

		if (defibulationsRemaining == 0) {
            DoctorEvents.Instance.PatientCriticalAdverted();
		}
	}
}
