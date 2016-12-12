using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCircle : MonoBehaviour {
    enum PlayerCircleStates { APPEARING, SHOWING, DISAPPEARING, NOTHING}
    PlayerCircleStates current_state = PlayerCircleStates.NOTHING;
    public Sprite[] doctorIconImages;
    public Sprite[] doctorIconImagesNoCheck;
    public float timeToAppear = 0.5f;
    public float timeToDissapear = 0.5f;
    private float startTime;
    private RectTransform rectTrans;

    // Use this for initialization
    void Start () {
        rectTrans = GetComponent<RectTransform>();
        rectTrans.localScale = Vector3.zero;
        startTime = Time.time;
        current_state = PlayerCircleStates.NOTHING;
	}
	
	// Update is called once per frame
	void Update () {
	    if(current_state == PlayerCircleStates.APPEARING) {
            var t = (Time.time - startTime) / timeToAppear;
            if(t >= 1.0f) {
                current_state = PlayerCircleStates.SHOWING;
                startTime = Time.time;
            } else {
                var newScale = Mathfx.Hermite(Vector3.zero, Vector3.one, t);
                rectTrans.localScale = newScale;
            }
        } else if(current_state == PlayerCircleStates.DISAPPEARING) {
            var t = (Time.time - startTime) / timeToAppear;
            if(t >= 0.0f) {
                current_state = PlayerCircleStates.NOTHING;
                startTime = Time.time;
            } else {
                var newScale = Mathfx.Hermite(Vector3.one, Vector3.zero, t);
                rectTrans.localScale = newScale;
            }
        }
	}

    public void SetPlayerNumAndInitiateAnimation(int playerNum, bool check) {
        SetPlayerNumNoAnimation(playerNum, check);
        current_state = PlayerCircleStates.APPEARING;
        startTime = Time.time;
        rectTrans.localScale = Vector3.zero;
    }

    public void SetPlayerNumNoAnimation(int playerNum, bool check) {
        if (check) {
            this.GetComponent<Image>().sprite = doctorIconImages[playerNum];
        } else {
            this.GetComponent<Image>().sprite = doctorIconImagesNoCheck[playerNum];
        }
    }

    public void RemoveIcon() {
        current_state = PlayerCircleStates.DISAPPEARING;
        startTime = Time.time;
    }
}
