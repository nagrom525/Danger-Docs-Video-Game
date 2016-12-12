using UnityEngine;
using System.Collections;

public class StepCompletePanel : MonoBehaviour {
    private enum StepCompletePanelState { ENTERING, SHOWING, WAITING_TO_LEAVE, LEAVING, GONE}
    private StepCompletePanelState current_state = StepCompletePanelState.ENTERING;

    public float timeToEnter = 0.5f;
    public float timeToLeave = 0.5f;
    public float timeToWaitToLeave = 0.5f;
    public GameObject[] playerCircles;

    private float timeLastState = 0.0f;
    private Vector3 rect3posOrig;
    private Vector3 startPos;
    private RectTransform rectTrans;

	// Use this for initialization
	void Start () {
        timeLastState = Time.time;
        rectTrans = GetComponent<RectTransform>();
        rect3posOrig = rectTrans.position;
        startPos = new Vector3(rect3posOrig.x, rect3posOrig.y - 200, rect3posOrig.z);
        rectTrans.position = startPos;
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
 
    }

    private void WaitingToLeaveUpdate() {
        var t = getT(timeLastState, timeToWaitToLeave);
        if(t >= 1.0) {
            current_state = StepCompletePanelState.WAITING_TO_LEAVE;
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
}
