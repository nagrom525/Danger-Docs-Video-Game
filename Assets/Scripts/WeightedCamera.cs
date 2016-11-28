using UnityEngine;
using System.Collections;

public class WeightedCamera : MonoBehaviour {

	public GameObject[] 	trackedObjs;
	public Camera 			cam;
	public float 			speed;

	public Vector2 			widthRange;
	public Vector2 			heightRange;

	public float 			widthDistance;
	public float 			maxWidthDistance;

	public Transform 		target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		widthRange.y = Mathf.Max(trackedObjs[0].transform.position.x, trackedObjs[1].transform.position.x, trackedObjs[2].transform.position.x, trackedObjs[3].transform.position.x);
		widthRange.x = Mathf.Min(trackedObjs[0].transform.position.x, trackedObjs[1].transform.position.x, trackedObjs[2].transform.position.x, trackedObjs[3].transform.position.x);

		heightRange.x = Mathf.Max(trackedObjs[0].transform.position.z, trackedObjs[1].transform.position.z, trackedObjs[2].transform.position.z, trackedObjs[3].transform.position.z);
		heightRange.y = Mathf.Min(trackedObjs[0].transform.position.z, trackedObjs[1].transform.position.z, trackedObjs[2].transform.position.z, trackedObjs[3].transform.position.z);

		widthDistance = Mathf.Abs(widthRange.y - widthRange.x);
		cam.fieldOfView = Mathf.Lerp( 20, 50, widthDistance/maxWidthDistance );

		float widthAvg = (widthRange.x + widthRange.y) / 2;
		float heightAvg = (heightRange.x + heightRange.y) / 2;
		target.position = new Vector3(widthAvg, 0f, heightAvg);
		cam.transform.LookAt(target);
	}
}
