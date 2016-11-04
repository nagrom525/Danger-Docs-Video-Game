using UnityEngine;
using System.Collections;

public class Doctor : MonoBehaviour {
    // CurrentTool can only be set by the Doctor when it interacts with a tool
    public Tool.ToolType CurentTool {get; private set;}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
