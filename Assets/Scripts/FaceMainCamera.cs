using UnityEngine;
using System.Collections;

public class FaceMainCamera : MonoBehaviour {
	public Camera targetCamera;

	// Use this for initialization
	void Start () {
		targetCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(targetCamera.transform.position);
	}
}
