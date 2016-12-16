using UnityEngine;
using System.Collections;
using System;

// delegates to send events to doctors / UI
// calls delegates based on timing in levels, critical events
// informs the doctor / UI that some has occured
// main level event code
public class DoctorEvents : MonoBehaviour {
    // States handlers ///
    public enum MainReciepeState { CUT_OPEN, PULL_OUT_STICK, SOAK_BLOOD, STICH_BODY}
    public enum PatientCriticalState { NORMAL, PATIENT_CRITICAL, POST_PATIENT_CRITICAL, GAME_OVER}
    public MainReciepeState[] scene1ReciepeElements = new MainReciepeState[4] { MainReciepeState.CUT_OPEN, MainReciepeState.PULL_OUT_STICK, MainReciepeState.SOAK_BLOOD, MainReciepeState.STICH_BODY };
    private PatientCriticalState gameState = PatientCriticalState.NORMAL;

    private int currentIndexInReciepe = -1; // must start @ -1
    private bool inRecipePostState = true;

    // Doctors / UI elements should register with the events they care about
    public delegate void DoctorEvent(float duration);
    public delegate void BucketEvent(bool full);
    public delegate void ToolEvent(Tool.ToolType type);

    // Cut open patient event
    public DoctorEvent patientNeedsCutOpen;
    public DoctorEvent patientDoneCutOpen;

    // Pull out stick event
    public DoctorEvent patientNeedsPullOutStick;
    public DoctorEvent patientDonePullOutStick;

    // Soak blood event
    public DoctorEvent patientNeedsBloodSoak;
    public DoctorEvent patientDoneBloodSoak;

    // Stitch Patient
    public DoctorEvent patientNeedsStitches;
    public DoctorEvent patientDoneStitches;

    // -- Patient Critical Events -- //
    public DoctorEvent onPatientCriticalEventStart;
    public DoctorEvent onPatientCriticalEventEnded;
    public DoctorEvent onPatientAboutToDie;

    // -- Patient Dead Events -- // 

    // -- Game over event -- //
    public DoctorEvent GameOver;
    // -- Game won -- //
    public DoctorEvent GameWon;


    // -- Spawning events -- //
    public DoctorEvent onBearAttack; // duration is time to wait before bear attack
    public DoctorEvent onRaccoonAttack; // duration is time to wait before raccoon attack
    public DoctorEvent onFire; // duration is time to wait before fire
    public DoctorEvent onFirePutOut; // called when the last flame has been extinguished
    public DoctorEvent onBearLeft; // called when the bear has left the doctors alone


    // -- Guidence Events -- //
    public DoctorEvent onAnestheticMachineLow;
    public DoctorEvent onAnestheticMachineReturned;
    public DoctorEvent onDoctorNeedsToWashHands;
    public DoctorEvent onDoctorWashedHands;

    // -- Tool Events
    public ToolEvent onToolPickedUpForSurgery;
    public ToolEvent onToolPickedUpGeneral;
    public ToolEvent onToolDroppedGeneral;
    public DoctorEvent onToolPickedUpCanister;
    public DoctorEvent onToolDroppedCanister;
    public ToolEvent onToolDroppedForSurgery;

    // -- Surgery Events -- //
    public DoctorEvent onSurgeryOperationFirst;
    public DoctorEvent onSurgeryOperationLeftLast;
    public DoctorEvent onSurgeryOperationComplete;

    // -- Bucket Events -- //
    public BucketEvent onBucketPickedUp;
    public DoctorEvent onBucketFilled;
    public BucketEvent onBucketDropped;
    public DoctorEvent onBucketEmptied;

    // Bear Events -- //
    public DoctorEvent onBearStealsPatient;

    public GameObject 	parachutePrefab;
	public Transform[] 	toolSpawnPoints;

    

    // --- Main Reciepe Values --//
    public float timeDelayGameStart = 2.0f;
    public float timeDelayPostCutOpen = 2.0f;
    public float timeDelayPostStickPullOut = 2.0f;
    public float timeDealyPostSoakBlood = 2.0f;
    public float timeDelayPostStiches = 2.0f;

    private float timeStartReciepeState = 0.0f;




    // ----------- Heart Attack Values ---------------------- //
    // ONE THING WE CAN DO: is start the heart attack duration at a base time, and add some kind of randomness to the rest of the time.
    //                      That way we have a base time that the heart rate would last, but the duration of the heart attack might still seem a bit random

    public float probabiltyPatientCritical = 0.05f; // probability of heart attack per second 
    public float postPatientCriticalDuration = 5.0f; // time to wait after heartattack is done before checking for another heart attack
    public float patientCriticalDuration = 10.0f; // lengh of time that a heart attack occurs
    private  float patientCriticalStartTime = 0.0f; // variable to store start time of a heart attack state
    private float lastTimePatientCriticalChecked = 0.0f;
    private Tool.ToolType activeTool = Tool.ToolType.NONE;
    private int numDoctorsOperating = 0;


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
		Time.timeScale = 1f;
        TutorialEventController.Instance.OnHeartAttackStart += OnTutorialHeartAttack;

	}

    void Update() {
        if (!TutorialEventController.Instance.tutorialActive) {
            switch (gameState) {
                case PatientCriticalState.NORMAL:
                    PatientCriticalUpdate();
                    break;
                case PatientCriticalState.PATIENT_CRITICAL:
                    GamePatientCriticalUpdate();
                    break;
                case PatientCriticalState.POST_PATIENT_CRITICAL:
                    GamePostPatientCriticalUpdate();
                    break;
            }

            switch (inRecipePostState) {
                case true:
                    RecipePostStateUpdate();
                    break;
                case false:
                    break;
            }
        }
    }

    private void PatientCriticalUpdate() {
        if ((Time.time - lastTimePatientCriticalChecked) >= 1.0f) {
            lastTimePatientCriticalChecked = Time.time;

            if (UnityEngine.Random.value < probabiltyPatientCritical) {
                gameState = PatientCriticalState.PATIENT_CRITICAL;
                patientCriticalStartTime = Time.time;
                if (onPatientCriticalEventStart != null) {
                    onPatientCriticalEventStart(patientCriticalDuration);
                }
            }
        }
    }

    private void RecipePostStateUpdate() {
        // this is the current reciepe state we are in
        float postDelayTime = 0.0f;
		if (currentIndexInReciepe == -1)
		{
			postDelayTime = timeDelayGameStart;
		}
		else {
			switch (scene1ReciepeElements[currentIndexInReciepe])
			{
				case MainReciepeState.CUT_OPEN:
					postDelayTime = timeDelayPostCutOpen;
					break;
				case MainReciepeState.PULL_OUT_STICK:
					postDelayTime = timeDelayPostStickPullOut;
					break;
				case MainReciepeState.SOAK_BLOOD:
					postDelayTime = timeDealyPostSoakBlood;
					break;
				case MainReciepeState.STICH_BODY:
					postDelayTime = timeDelayPostStiches;
					break;
			}
		}
        
        if((Time.time - timeStartReciepeState) > postDelayTime) {
            currentIndexInReciepe++;
			CallRecipeEventFunction(scene1ReciepeElements[currentIndexInReciepe]);
			inRecipePostState = false;
        }         
    }

    private void GamePatientCriticalUpdate() {
        if ((Time.time - patientCriticalStartTime) > patientCriticalDuration) {
            EndPatientCritical();
            InducePatientDeath();
        }
    }


    private void GamePostPatientCriticalUpdate() {
        if ((Time.time - patientCriticalStartTime) > postPatientCriticalDuration) {
            gameState = gameState = PatientCriticalState.NORMAL;
            lastTimePatientCriticalChecked = Time.time;
            patientCriticalStartTime = Time.time;
        }

    }


    private void EndPatientCritical() {
        gameState = PatientCriticalState.POST_PATIENT_CRITICAL;
        patientCriticalStartTime = Time.time;
        if(onPatientCriticalEventEnded != null) {
            onPatientCriticalEventEnded(postPatientCriticalDuration);
        }

    }

    // called when the game is supposed to end (either prematurly or due to the players running out of time due to anesthetic)
    // gives the player one more chance by sending the patient into critical
    // will do nothing if the patient was just critical or is still critical
    public void InducePatientCritical() {
		Debug.Log("InducePatientCritical()");
		if(gameState == PatientCriticalState.NORMAL) {
            gameState = PatientCriticalState.PATIENT_CRITICAL;
            patientCriticalStartTime = Time.time;
            if (onPatientCriticalEventStart != null) {
                onPatientCriticalEventStart(patientCriticalDuration);
            }
        }
    }

    public void PatientCriticalAdverted() {
        if (gameState == PatientCriticalState.PATIENT_CRITICAL) {
            EndPatientCritical();
        } else {
            Debug.Log("Heart Attack adverted when the patient wasn't in a Heart Attack");
        }
    }

    // actually ends the game 
    // if the player critical state isn't adverted
    public void InducePatientDeath() {
        Time.timeScale = 0.0f;
        gameState = PatientCriticalState.GAME_OVER;
        if (GameOver != null) {
            GameOver(0.0f);
        }
    }

    private Tool.ToolType RequiredToolForReciepeState(MainReciepeState recipeState) {
        switch (recipeState) {
            case MainReciepeState.CUT_OPEN:
                return Tool.ToolType.SCALPEL;
            case MainReciepeState.PULL_OUT_STICK:
                return Tool.ToolType.FORCEPS;
            case MainReciepeState.SOAK_BLOOD:
                return Tool.ToolType.GAUZE;
            case MainReciepeState.STICH_BODY:
                return Tool.ToolType.SUTURE;
            default:
                return Tool.ToolType.NONE;
        }
    }

    //////         ----------- Main Event Public Interface -----------              ///////////////
    public void OnPatientCutOpen() {
        if (!TutorialEventController.Instance.tutorialActive) {
            SetRecipePostState();
            if (patientDoneCutOpen != null) {
                patientDoneCutOpen(0);
            }
            if(onSurgeryOperationComplete != null) {
                onSurgeryOperationComplete(0.0f);
            }
        }
    }

    public void OnPatientStickPulledOut() {
        if (!TutorialEventController.Instance.tutorialActive) {
            SetRecipePostState();
            if (patientDonePullOutStick != null) {
                patientDonePullOutStick(0);
            }
            if (onSurgeryOperationComplete != null) {
                onSurgeryOperationComplete(0.0f);
            }
        }
    }

    public void OnPatientBloodSoaked() {
        if (!TutorialEventController.Instance.tutorialActive) {
            SetRecipePostState();
            if (patientDoneBloodSoak != null) {
                patientDoneBloodSoak(0);
            }
            if (onSurgeryOperationComplete != null) {
                onSurgeryOperationComplete(0.0f);
            }
        }
    }

    public void OnPatientStitched() {
        if (!TutorialEventController.Instance.tutorialActive) {
            SetRecipePostState();
            if (patientDoneStitches != null) {
                patientDoneStitches(0);
            }
            if (onSurgeryOperationComplete != null) {
                onSurgeryOperationComplete(0.0f);
            }
        }
    }

    public void SetRecipePostState() {
        if (!TutorialEventController.Instance.tutorialActive) {
            inRecipePostState = true;
            timeStartReciepeState = Time.time;
            if (currentIndexInReciepe == scene1ReciepeElements.Length - 1) {
                // then we have won the game!
                if (GameWon != null) {
                    GameWon(0);
                }
            }
        }
    }

    public void InformBearAttack(float duration) {
        if(onBearAttack != null) {
            onBearAttack(duration);
        }
    }

    public void InformFire(float duration) {
        if(onFire != null) {
            onFire(duration);
        }
    }

    public void InformRacconAttack(float duration) {
        if(onRaccoonAttack != null) {
            onRaccoonAttack(duration);
        }
    }

    public void InformAnestheticMachineLow(float precentLeft) {
        if (!TutorialEventController.Instance.tutorialActive) {
            if (onAnestheticMachineLow != null) {
                onAnestheticMachineLow(precentLeft);
            }
        }
    }

    public void InformAnestheticMachineReturnedHigh(float precentLeft) {
        if (!TutorialEventController.Instance.tutorialActive) {
            if (onAnestheticMachineReturned != null) {
                onAnestheticMachineReturned(precentLeft);
            }
        }
    }

    public void InformDoctorNeedsToWashHands(float duration) {
        if(onDoctorNeedsToWashHands != null && !TutorialEventController.Instance.tutorialActive) {
            onDoctorNeedsToWashHands(duration);
        }
    }

    public void InformToolPickedUp(Tool.ToolType toolType, bool full) {
        if ((currentIndexInReciepe != -1) && (toolType == RequiredToolForReciepeState(scene1ReciepeElements[currentIndexInReciepe]))) {
            if (onToolPickedUpForSurgery != null) {
                onToolPickedUpForSurgery(toolType);
            }
        } else if (toolType == Tool.ToolType.CANISTER) {
            if (onToolPickedUpCanister != null) {
                onToolPickedUpCanister(0);
            }
        } else if(toolType == Tool.ToolType.BUCKET) {
            if(onBucketPickedUp != null) {
                onBucketPickedUp(full);
            }
        } else {
            if(onToolPickedUpGeneral != null) {
                onToolPickedUpGeneral(toolType);
            }
        }
    }

	public void InformToolTaken(Tool tool) {
		switch (tool.GetToolType()) {
			case Tool.ToolType.GAUZE:
				//spawn gauze drop
				SpawnGauze();
				break;
			case Tool.ToolType.BUCKET:
				//spawn gauze drop
				SpawnBucket();
				break;
			case Tool.ToolType.SUTURE:
				//spawn gauze drop
				SpawnSuture();
				break;
			case Tool.ToolType.CANISTER:
				//spawn gauze drop
				SpawnCanister();
				break;
			case Tool.ToolType.SCALPEL:
				//spawn gauze drop
				SpawnScalpel();
				break;
			case Tool.ToolType.DEFIBULATOR:
				//spawn gauze drop
				SpawnDefibulator();
				break;
		}
	}

	public void InformFirePutOut() {
        if(onFirePutOut != null) {
            onFirePutOut(0);
        }
    }

    public void InformBearLeft() {
        if(onBearLeft != null) {
            onBearLeft(0);
        }
    }

    public void InformToolDropped(Tool.ToolType type, bool full) {
        if(type == Tool.ToolType.CANISTER) {
            if(onToolDroppedCanister != null) {
                onToolDroppedCanister(0);
            }
        } else if((currentIndexInReciepe != -1) && (type == RequiredToolForReciepeState(scene1ReciepeElements[currentIndexInReciepe]))){
            if(onToolDroppedForSurgery != null) {
                onToolDroppedForSurgery(type);
            }
        } else if(type == Tool.ToolType.BUCKET) {
            onBucketDropped(full);
        } else {
            if(onToolDroppedGeneral != null) {
                onToolDroppedGeneral(type);
            }
        }
    }


    public void InformSurgeryOperation() {
        if (!TutorialEventController.Instance.tutorialActive) {
            if ((numDoctorsOperating == 0) && (onSurgeryOperationFirst != null)) {
                onSurgeryOperationFirst(0);
            }
            ++numDoctorsOperating;
        }
    }

    public void InformDoctorLeftSurgeryOperaton() {
        if (!TutorialEventController.Instance.tutorialActive) {
            --numDoctorsOperating;
            if ((numDoctorsOperating == 0) && (onSurgeryOperationLeftLast != null)) {
                onSurgeryOperationLeftLast(0);
            }
        }
    }

    public void InformPatientAboutToDie(float timeLeftToLive) {
        if(onPatientAboutToDie != null) {
            onPatientAboutToDie(timeLeftToLive);
        }
    }

    public void InformDoctorWashedHands() {
        if(onDoctorWashedHands != null) {
            onDoctorWashedHands(0.0f);
        }
    }

    public void InformBucketFilled() {
        if(onBucketFilled != null) {
            onBucketFilled(0.0f);
        }
    }

    public void InformBucketEmptied() {
        if(onBucketEmptied != null) {
            onBucketEmptied(0.0f);
        }
    }

    public void InformBearStealingPatient() {
        if(onBearStealsPatient != null) {
            onBearStealsPatient(0.0f);
        }
    }

    private void CallRecipeEventFunction(MainReciepeState reciepeState) {
        switch (reciepeState) {
            case MainReciepeState.CUT_OPEN:
                if(patientNeedsCutOpen != null) {
                    patientNeedsCutOpen(0);
                }
                break;
            case MainReciepeState.PULL_OUT_STICK:
                if (patientNeedsPullOutStick != null) {
                    patientNeedsPullOutStick(0);
                }
                break;
            case MainReciepeState.SOAK_BLOOD:
                if (patientNeedsBloodSoak != null) {
                    patientNeedsBloodSoak(0);
                }
                break;
            case MainReciepeState.STICH_BODY:
                if (patientNeedsStitches != null) {
                    patientNeedsStitches(0);
                }
                break;
        }
    }

	// -- Spawn Stolen Tools -- //
	void SpawnGauze()
	{
		var go = (GameObject)Instantiate(parachutePrefab, toolSpawnPoints[0]);
		go.transform.parent = null;
		go.transform.position = toolSpawnPoints[0].position + RandomSpawnOffset();
		go.GetComponent<ToolDrop>().type = Tool.ToolType.GAUZE;
	}

	void SpawnSuture()
	{
		var go = (GameObject)Instantiate(parachutePrefab, toolSpawnPoints[1]);
		go.transform.parent = null;
		go.transform.position = toolSpawnPoints[1].position + RandomSpawnOffset();
		go.GetComponent<ToolDrop>().type = Tool.ToolType.SUTURE;
	}

	void SpawnCanister()
	{
		var go = (GameObject)Instantiate(parachutePrefab, toolSpawnPoints[2]);
		go.transform.parent = null;
		go.transform.position = toolSpawnPoints[2].position + RandomSpawnOffset();
		go.GetComponent<ToolDrop>().type = Tool.ToolType.CANISTER;
	}

	void SpawnDefibulator()
	{
		var go = (GameObject)Instantiate(parachutePrefab, toolSpawnPoints[3]);
		go.transform.parent = null;
		go.transform.position = toolSpawnPoints[3].position + RandomSpawnOffset();
		go.GetComponent<ToolDrop>().type = Tool.ToolType.DEFIBULATOR;
	}


	void SpawnScalpel()
	{
		var go = (GameObject)Instantiate(parachutePrefab, toolSpawnPoints[4]);
		go.transform.parent = null;
		go.transform.position = toolSpawnPoints[4].position + RandomSpawnOffset();
		go.GetComponent<ToolDrop>().type = Tool.ToolType.SCALPEL;
	}

	void SpawnBucket()
	{
		var go = (GameObject)Instantiate(parachutePrefab, toolSpawnPoints[5]);
		go.transform.parent = null;
		go.transform.position = toolSpawnPoints[5].position + RandomSpawnOffset();
		go.GetComponent<ToolDrop>().type = Tool.ToolType.BUCKET;
	}

	Vector3 RandomSpawnOffset()
	{
		return new Vector3(UnityEngine.Random.Range(-3f, 3f), 0f, UnityEngine.Random.Range(-3f, 3f));
	}

    private void OnTutorialHeartAttack() {
        InducePatientCritical();
    }

    public float GetSurgeryPrecentComplete() {
        float realIndex = currentIndexInReciepe;
        if (inRecipePostState) {
            realIndex += 1;
        }
        return realIndex / ((float)scene1ReciepeElements.Length);
    }

    private void OnDestroy() {
        TutorialEventController.Instance.OnHeartAttackStart -= OnTutorialHeartAttack;
    }
}
