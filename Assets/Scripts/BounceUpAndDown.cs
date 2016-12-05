using UnityEngine;
using System.Collections;

public class BounceUpAndDown : MonoBehaviour {
    public float bounceTime = 2.0f;
    public float bounceHeight = 2.0f;

    private bool bounceActive = false;
    private float startTime = 0.0f;
    private Vector3 startPosition;

    void Awake() {
        startPosition = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        if (bounceActive) {
            float t = ((Time.time - startTime) / bounceTime);
            if(t >= 1.0f) {
                transform.localPosition = startPosition;
                bounceActive = false;
            }
            Vector3 currPos = transform.localPosition;
            transform.localPosition = new Vector3(currPos.x, startPosition.y + (Mathfx.Bounce(t) * bounceHeight), currPos.z);
        }
    }

    public void initiateBounce() {
        bounceActive = true;
        startTime = Time.time;
    }
}
