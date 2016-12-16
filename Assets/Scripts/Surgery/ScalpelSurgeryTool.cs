using UnityEngine;
using System.Collections;

public class ScalpelSurgeryTool : SurgeryTool {

	public Transform model;
	public Vector3 idlePos;
	public Vector3 activatedPos;

	public bool touchedStart;
	public bool touchedEnd;
	public bool inMidTrack;

	public void Reset()
	{
		touchedStart = false;
		touchedEnd = false;
		inMidTrack = false;

	}

	void Start()
	{
		model = transform.GetChild(0);
		idlePos = model.localPosition;
        idlePos = new Vector3(0f,0f,0f);
	}


	public override void Activate()
	{
		base.Activate();
		model.localPosition = activatedPos;
	}

	public override void Deactivate()
	{
		base.Deactivate();
		model.localPosition = idlePos;
		Reset();
	}


}
