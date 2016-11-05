using UnityEngine;
using System.Collections;

public class TempMovement : MonoBehaviour {
	public GameObject target;
	public float speed = 3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Basic movement
		if (Input.GetKey(KeyCode.A)) {
			target.transform.position += Vector3.left * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S)) {
			target.transform.position += Vector3.back * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.D)) {
			target.transform.position += Vector3.right * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.W)) {
			target.transform.position += Vector3.forward * speed * Time.deltaTime;
		}
	}
}
