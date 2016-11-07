using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;

public class LevelUserInterface : MonoBehaviour {
	public Text heartrate;
	public Image EventSignal;
	public float doctorBlinkDuration;
	// Use this for initialization
	void Start () {
	    // We probably want to register private member functions with DoctorEvents delegates
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
