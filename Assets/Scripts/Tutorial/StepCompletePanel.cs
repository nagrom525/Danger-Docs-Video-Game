using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StepCompletePanel : MonoBehaviour {
    private enum StepCompletePanelState { ENTERING, SHOWING, WAITING_TO_LEAVE, LEAVING, GONE}
    private StepCompletePanelState current_state = StepCompletePanelState.ENTERING;
    public TutorialEventController.TutorialStates panel_type = TutorialEventController.TutorialStates.WASH_HANDS;

    public float timeToEnter = 0.5f;
    public float timeToLeave = 0.5f;
    public float timeToWaitToLeave = 0.5f;
    public GameObject[] playerCircles;
    private int[] circlesFilled = new int[4] { -1, -1, -1, -1 };
    private bool[] playerCircleShown = new bool[4];

    private float timeLastState = 0.0f;
    private Vector3 rect3posOrig;
    private Vector3 startPos;
    private RectTransform rectTrans;

    private List<int> playerBuffer = new List<int>();

	// Use this for initialization
	void Start () {
        timeLastState = Time.time;
        rectTrans = GetComponent<RectTransform>();
        rect3posOrig = rectTrans.position;
        startPos = new Vector3(rect3posOrig.x, rect3posOrig.y - 200, rect3posOrig.z);
        rectTrans.position = startPos;


        TutorialEventController.Instance.OnHandsWashed += OnHandsWashed;
        TutorialEventController.Instance.OnToolPickedUp += OnToolPickedUp;
        TutorialEventController.Instance.OnToolDropped += OnToolDropped;
        TutorialEventController.Instance.OnDoctorAtPatient += OnDoctorAtPatient;
        TutorialEventController.Instance.OnDoctorLeavesPatient += OnDoctorLeavesPatient;
        TutorialEventController.Instance.OnSurgeryComplete += OnSurgeryComplete;
        TutorialEventController.Instance.OnBatteryUsed += OnBatteryUsed;
        DoctorEvents.Instance.onFirePutOut += OnFirePutOut;
        DoctorEvents.Instance.onPatientCriticalEventEnded += OnHeartAttackAdverted;
        TutorialEventController.Instance.OnPlayerScaredRaccoon += OnScareAwayRaccoon;
        TutorialEventController.Instance.OnPlayerScaredBear += OnScareAwayBear;
    }
	
	// Update is called once per frame
	void Update () {
        switch (current_state) {
            case StepCompletePanelState.ENTERING:
                EnteringUpdate();
                break;
            case StepCompletePanelState.SHOWING:
                ShowingUpdate();
                break;
            case StepCompletePanelState.WAITING_TO_LEAVE:
                WaitingToLeaveUpdate();
                break;
            case StepCompletePanelState.LEAVING:
                LeavingUpdate();
                break;
        }
	}

    private void EnteringUpdate() {
        var t = getT(timeLastState, timeToEnter);
        if(t >= 1.0) {
            current_state = StepCompletePanelState.SHOWING;
            timeLastState = Time.time;
        }
        Vector3 newPos = Mathfx.Hermite(startPos, rect3posOrig, t);
        rectTrans.position = newPos;
    }

    private void ShowingUpdate() {
        if (playerBuffer.Count != 0) {
            foreach(var playerNum in playerBuffer) {
                AddPlayerCircle(playerNum);
            }
            playerBuffer.Clear();
        }
        foreach(var playerShown in playerCircleShown) {
            if (!playerShown) {
                return;
            }
        }
        current_state = StepCompletePanelState.WAITING_TO_LEAVE;
        timeLastState = Time.time;
    }

    private void WaitingToLeaveUpdate() {
        var t = getT(timeLastState, timeToWaitToLeave);
        if(t >= 1.0) {
            current_state = StepCompletePanelState.LEAVING;
            timeLastState = Time.time;
        }
    }

    private void LeavingUpdate() {
        var t = getT(timeLastState, timeToLeave);
        if(t >= 1.0) {
            current_state = StepCompletePanelState.GONE;
            timeLastState = Time.time;
            this.gameObject.SetActive(false);
        }
        Vector3 newPos = Mathfx.Hermite(rect3posOrig, startPos, t);
        rectTrans.position = newPos;
    }

    private float getT(float timeStart, float timeTotal) {
        return (Time.time - timeStart) / timeTotal;
    }

    private void AddPlayerCircle(int playerNum) {
        if (playerCircleShown[playerNum]) {
            return;
        }
        if (current_state != StepCompletePanelState.SHOWING) {
            playerBuffer.Add(playerNum);
        } else {
            for (int i = 0; i < circlesFilled.Length; ++i) {
                if (circlesFilled[i] == -1) {
                    circlesFilled[i] = playerNum;
                    playerCircleShown[playerNum] = true;
                    playerCircles[i].GetComponent<PlayerCircle>().SetPlayerNumAndInitiateAnimation(playerNum);
                    return;
                }
            }
        }
    }

    private void RemovePlayerCircle(int playerNum) {
        if (!playerCircleShown[playerNum]) {
            return;
        }
        for(int i = 0; i < circlesFilled.Length; ++i) {
            if(circlesFilled[i] == playerNum) {
                playerCircleShown[playerNum] = false;
                circlesFilled[i] = -1;
                playerCircles[i].GetComponent<PlayerCircle>().RemoveIcon();
            }
        }

    }

    private void OnHandsWashed(float precent, int playerNum) {
        if (panel_type == TutorialEventController.TutorialStates.WASH_HANDS) {
            if(precent >= 0.999990) {
                AddPlayerCircle(playerNum);
            }
        }
    }

    private void OnToolPickedUp(Tool.ToolType type, int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.PICK_UP_TOOL_GO_TO_PATIENT) {
            AddPlayerCircle(playerNum);
        }
    }

    private void OnToolDropped(Tool.ToolType type, int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.PICK_UP_TOOL_GO_TO_PATIENT) {
            RemovePlayerCircle(playerNum);
        }
    }

    private void OnDoctorAtPatient(int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.PICK_UP_TOOL_GO_TO_PATIENT) {
            AddPlayerCircle(playerNum);
        }
    }

    private void OnDoctorLeavesPatient(int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.PICK_UP_TOOL_GO_TO_PATIENT) {
            RemovePlayerCircle(playerNum);
        }
    }

    private void OnSurgeryComplete(int playerNum) {

    }

    private void OnBatteryUsed(int playerNum) {

    }

    private void OnFirePutOut(float duration) {

    }

    private void OnHeartAttackAdverted(float duration) {

    }

    private void OnScareAwayRaccoon(int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.SCARE_AWAY_RACCON) {
            AddPlayerCircle(playerNum);
        }
    }

    private void OnScareAwayBear(int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.SCARE_AWAY_BEAR) {
            AddPlayerCircle(playerNum);
        }
    }

    void OnDestroy() {
        TutorialEventController.Instance.OnHandsWashed -= OnHandsWashed;
        TutorialEventController.Instance.OnToolPickedUp -= OnToolPickedUp;
        TutorialEventController.Instance.OnToolDropped -= OnToolDropped;
        TutorialEventController.Instance.OnDoctorAtPatient -= OnDoctorAtPatient;
        TutorialEventController.Instance.OnDoctorLeavesPatient -= OnDoctorLeavesPatient;
        TutorialEventController.Instance.OnSurgeryComplete -= OnSurgeryComplete;
        TutorialEventController.Instance.OnBatteryUsed -= OnBatteryUsed;
        DoctorEvents.Instance.onFirePutOut -= OnFirePutOut;
        DoctorEvents.Instance.onPatientCriticalEventEnded -= OnHeartAttackAdverted;
        TutorialEventController.Instance.OnPlayerScaredRaccoon -= OnScareAwayRaccoon;
        TutorialEventController.Instance.OnPlayerScaredBear -= OnScareAwayBear;
    }
}
