using UnityEngine;
using System.Collections;

public class ScalpelTrack : MonoBehaviour {

	public bool isStart;
	public bool isEnd;
	public bool isMidTrack;

	public void Activate()
	{
		GetComponent<Renderer>().material.color = Color.green;
	}

	public void Deactivate()
	{
		GetComponent<Renderer>().material.color = Color.red;
	}



	// Use this for initialization
	void Start () {
		Deactivate();

		if (isStart)
			GetComponent<Renderer>().material.color = Color.blue;

		if (isEnd)
			GetComponent<Renderer>().material.color = Color.magenta;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
