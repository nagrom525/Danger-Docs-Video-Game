using UnityEngine;
using System.Collections;

public class Doctor : MonoBehaviour {
    // CurrentTool can only be set by the Doctor when it interacts with a tool
    public Tool curentTool {get; private set;}

	// Use this for initialization
	void Start () {
	    // NEED to register event listner functions to the DoctorEvents singleton delegates
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Doctor control code (needs to be added)
    // for example... 
    public void OnJumpKeyPressed() {

    }

}
