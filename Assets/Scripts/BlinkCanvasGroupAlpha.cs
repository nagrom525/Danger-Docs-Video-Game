using UnityEngine;
using System.Collections;

public class BlinkCanvasGroupAlpha : MonoBehaviour {

    public CanvasGroup CG;
    private float t;
    public float speed = 5f;

	// Use this for initialization
	void Start () {
        CG = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        CG.alpha = Mathf.Sin(speed*t);
	}
}
