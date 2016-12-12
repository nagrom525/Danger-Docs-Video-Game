using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCircle : MonoBehaviour {
    enum PlayerCircleStates { APPEARING, SHOWING, NOTHING}
    PlayerCircleStates current_state = PlayerCircleStates.NOTHING;
    public Sprite[] doctorIconImages;
    public float timeToAppear;
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
        }
	}

    public void SetPlayerNumAndInitiateAnimation(int playerNum) {
        this.GetComponent<Image>().sprite = doctorIconImages[playerNum];
        current_state = PlayerCircleStates.APPEARING;
        startTime = Time.time;
        rectTrans.localScale = Vector3.zero;
    }
}
