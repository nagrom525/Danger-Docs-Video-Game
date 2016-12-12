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
    private bool[] playerCirclesChecked = new bool[4];

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
        current_state = StepCompletePanelState.ENTERING;


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
                AddPlayerCircle(playerNum, true);
            }
            playerBuffer.Clear();
        }
        foreach(var playerShown in playerCircleShown) {
            if (!playerShown) {
                return;
            }
        }
        if(panel_type == TutorialEventController.TutorialStates.PICK_UP_TOOL_GO_TO_PATIENT) {
            foreach (var playerChecked in playerCirclesChecked) {
                if (!playerChecked) {
                    return;
                }
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

    private void AddPlayerCircle(int playerNum, bool check) {
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
                    playerCircles[i].GetComponent<PlayerCircle>().SetPlayerNumAndInitiateAnimation(playerNum, check);
                    return;
                }
            }
        }
    }

    private void SetPlayerCircleChecked(int playerNum, bool check) {
        if(check == true) {
            playerCirclesChecked[playerNum] = true;
        } else {
            playerCirclesChecked[playerNum] = false;
        }
        for(int i = 0; i < circlesFilled.Length; ++i) {
            if (playerNum == circlesFilled[i]) {
                    playerCircles[i].GetComponent<PlayerCircle>().SetPlayerNumNoAnimation(playerNum, check);
                return;
            }
        }
    }

    private void AddPlayerCircle(int playerNum, Tool.ToolType toolType, bool check) {
        if (playerCircleShown[playerNum]) {
            return;
        }
        if(current_state != StepCompletePanelState.SHOWING) {
            playerBuffer.Add(playerNum);
        } else {
            int i = 0;
            if (toolType == Tool.ToolType.SCALPEL) {
                if(circlesFilled[i] != -1) {
                    i = 1;
                }
            } else if(toolType == Tool.ToolType.GAUZE) {
                i = 2;
            } else {
                i = 3;
            }
            circlesFilled[i] = playerNum;
            playerCircleShown[playerNum] = true;
            playerCircles[i].GetComponent<PlayerCircle>().SetPlayerNumAndInitiateAnimation(playerNum, check);
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
                AddPlayerCircle(playerNum, true);
            }
        }
    }

    private void OnToolPickedUp(Tool.ToolType type, int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.PICK_UP_TOOL_GO_TO_PATIENT) {
            AddPlayerCircle(playerNum, type, false);
        }
    }

    private void OnToolDropped(Tool.ToolType type, int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.PICK_UP_TOOL_GO_TO_PATIENT) {
            RemovePlayerCircle(playerNum);
        }
    }

    private void OnDoctorAtPatient(int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.PICK_UP_TOOL_GO_TO_PATIENT) {
            SetPlayerCircleChecked(playerNum, true);
        }
    }

    private void OnDoctorLeavesPatient(int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.PICK_UP_TOOL_GO_TO_PATIENT) {
            SetPlayerCircleChecked(playerNum, false);
        }
    }

    private void OnSurgeryComplete(int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.SURGERY_ON_PATIENT) {
            AddPlayerCircle(playerNum, true);
        }
    }

    private void OnBatteryUsed(int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.ANESTHETIC_MACHINE) {
            AddPlayerCircle(playerNum, true);
        }
    }

    private void OnFirePutOut(float duration) {
        //if(panel_type == TutorialEventController.TutorialStates.FIRE) {
           
        //}
    }

    private void OnHeartAttackAdverted(float duration) {
        //if(panel_type == TutorialEventController.TutorialStates.HEART_ATTACK) {

        //}
    }

    private void OnScareAwayRaccoon(int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.SCARE_AWAY_RACCON) {
            AddPlayerCircle(playerNum, true);
        }
    }

    private void OnScareAwayBear(int playerNum) {
        if(panel_type == TutorialEventController.TutorialStates.SCARE_AWAY_BEAR) {
            AddPlayerCircle(playerNum, true);
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
