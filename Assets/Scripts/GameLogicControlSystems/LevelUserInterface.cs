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
    public GameObject backButton;

    // -- Heart Monitor -- //
    public GameObject heartMonitorAnimation;
    public GameObject heartMonitor;
    public string normalHeartMonitorSpritesName;
    public string attackingHeartMonitorSpritesName;
    public Sprite normalHeartMonitorSprite;
    public Sprite attackingHeartMonitorSprite;
    // -- // -- //

    private StatusIndicatorState status_state = StatusIndicatorState.INACTIVE;
    private float statusIndicatorStart = 0.0f;
    private float statusIndicatorLastBlinkTime = 0.0f;

	public Image heartrateindicator;
	public bool heartrateindictatoron = false;
	private float t = 0f;
	private bool done;
	private Vector2 start;
	private Vector2 end;
	private float timedelta1 = 1.0f;
	private float timedelta2 = 0.2f;
    private float bpm;
    private bool heartMonitorActive = true;
    private SpriteAnimator heartRateAnimator;



    public static LevelUserInterface UI;

	void Awake() {
		if (UI == null) {
			UI = this;
		} else {
			Debug.Log("UI only be set once");
		}
        heartRateAnimator = heartMonitorAnimation.GetComponent<SpriteAnimator>();
	}
	// Use this for initialization
	void Start () {
        // We probably want to register private member functions with DoctorEvents delegates
		DoctorEvents.Instance.onPatientAboutToDie += OnPatientAboutToDie;
        DoctorEvents.Instance.onPatientCriticalEventStart += OnPatientCriticalEventStart;
		DoctorEvents.Instance.onPatientCriticalEventEnded += OnPatientCriticalEventEnded;
 
        DoctorEvents.Instance.GameOver += OnGameOver;
		DoctorEvents.Instance.GameWon += OnGameWon;
        DoctorEvents.Instance.onSurgeryOperationLeftLast += OnLastDoctorLeavesSurgery;
        DoctorEvents.Instance.onSurgeryOperationFirst += OnFirstDoctorEntersSurgery;
		start = heartrateindicator.rectTransform.sizeDelta;
		end = heartrateindicator.rectTransform.sizeDelta - new Vector2(900.0f, 600f);

        SetHeartMonitorNormal();
	}
	
	// Update is called once per frame
	void Update () {
        if(status_state != StatusIndicatorState.INACTIVE) {
            StatusIndicatorActiveUpdate();
        }
		if (heartrateindictatoron && !done)
		{
			float timeSinceStarted = Time.time - t;
			float percentageComplete = timeSinceStarted / timedelta2;
			heartrateindicator.rectTransform.sizeDelta = Vector2.Lerp(start, end, percentageComplete);

			if (percentageComplete >= 1.0f)
			{
				t = Time.time;
				done = true;
			}
		}
		if (heartrateindictatoron && done)
		{
			float timeSinceStarted1 = Time.time - t;
			float percentageComplete1 = timeSinceStarted1 / timedelta1;
			heartrateindicator.rectTransform.sizeDelta = Vector2.Lerp(end, start, percentageComplete1);

			if (percentageComplete1 >= 1.0f)
			{
				done = false;
				t = Time.time;
			}

		}

        HeartMonitorUpdate();
	
	}

    // -- HEART MONITOR -- //
    public void SetHeartMonitorActive(bool active) {
        heartMonitor.gameObject.SetActive(active);
        heartMonitorActive = active;
    }

    private void HeartMonitorUpdate() {
        if (heartMonitorActive) {
            heartRateAnimator.fps = BpmToFps(bpm);
        }
    }


    private int BpmToFps(float bpm) {
        if (heartMonitorActive) {
            return Mathf.RoundToInt((bpm / 60.0f) * heartRateAnimator.numFrames);
        } else return -1;
    }

	public void UpdateBpm(float bpm) {
        this.bpm = bpm;
	}

    private void SetHeartMonitorNormal() {
        if (heartMonitorActive) {
            heartRateAnimator.updateFrames(normalHeartMonitorSpritesName);
            heartMonitor.GetComponent<Image>().sprite = normalHeartMonitorSprite;
            heartRateAnimator.startAnimation(true);
        }
    }

    private void SetHeartMonitorAttacking() {
        if (heartMonitorActive) {
            heartRateAnimator.updateFrames(attackingHeartMonitorSpritesName);
            heartMonitor.GetComponent<Image>().sprite = attackingHeartMonitorSprite;
            heartRateAnimator.startAnimation(true);
        }
    }

    // -- // -- //

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
	void OnPatientAboutToDie(float duration) {
		heartrateindicator.gameObject.SetActive(true);
		heartrateindictatoron = true;
		done = false;
		t = Time.time;
		print(start);
		print(end);
		//start = heartrateindicator.rectTransform.sizeDelta;
		//end = heartrateindicator.rectTransform.sizeDelta - new Vector2(900.0f, 600f);
    }

    void OnPatientCriticalEventStart(float duration) {
        SetHeartMonitorAttacking();
    }

	void OnPatientCriticalEventEnded(float duration) {
		heartrateindictatoron = false;
		heartrateindicator.gameObject.SetActive(false);
        SetHeartMonitorNormal();

	}

    void OnGameOver(float duration) {
        SceneTransitionController.Instance.GameLost();
        //gameLostPanel.SetActive(true);
    }

    void OnGameWon(float duration) {
        //gameWonPanel.SetActive(true);
        SceneTransitionController.Instance.GameWon();
    }

    private void OnFirstDoctorEntersSurgery(float duration) {
        backButton.SetActive(true);
    }

    private void OnLastDoctorLeavesSurgery(float duration) {
        backButton.SetActive(false);
    }

    void OnDestroy() {
        DoctorEvents.Instance.onPatientAboutToDie -= OnPatientAboutToDie;
        DoctorEvents.Instance.onPatientCriticalEventStart -= OnPatientCriticalEventStart;
        DoctorEvents.Instance.onPatientCriticalEventEnded -= OnPatientCriticalEventEnded;

        DoctorEvents.Instance.GameOver -= OnGameOver;
        DoctorEvents.Instance.GameWon -= OnGameWon;
        DoctorEvents.Instance.onSurgeryOperationLeftLast -= OnLastDoctorLeavesSurgery;
        DoctorEvents.Instance.onSurgeryOperationFirst -= OnFirstDoctorEntersSurgery;
    }
 

}
