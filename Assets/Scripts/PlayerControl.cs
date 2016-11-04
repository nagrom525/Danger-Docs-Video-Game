using UnityEngine;
using System.Collections;

// converts controler inputs to events in the doctor prefab
// LAYER OF ABSTRACTION
// calls functions based on player input
//      Abstracts away the fact that there are multiple controls
//      and multiple buttons on thouse controls
public class PlayerControl : MonoBehaviour {
    Doctor attachedDoctor;

    void Awake() {
        attachedDoctor = GetComponent<Doctor>();
    }
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        // example of design pattern:
        //if (Input.GetKeyDown(KeyCode.A)) {
        //    attachedDoctor.OnJumpKeyPressed();
        //}
    }
}
