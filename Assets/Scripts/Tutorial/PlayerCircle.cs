using UnityEngine;
using System.Collections;

public class PlayerCircle : MonoBehaviour {
    enum PlayerCircleStates { APPEARING, SHOWING}
    PlayerCircleStates current_state = PlayerCircleStates.APPEARING;
    public GameObject[] doctorIconPrefabs;
    public float timeToAppear;
    public float startTime;
    private RectTransform rectTrans;

    // Use this for initialization
    void Start () {
        rectTrans = GetComponent<RectTransform>();
        rectTrans.localScale = Vector3.zero;
        startTime = Time.time;
        current_state = PlayerCircleStates.APPEARING;
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
}
