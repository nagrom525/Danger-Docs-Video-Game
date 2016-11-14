using UnityEngine;
using System.Collections;

public class GauzeSurgeryTool : SurgeryTool {

	public Transform model;
	public Vector3 idlePos;
	public Vector3 activatedPos;


	void Start()
	{
		model = transform.GetChild(0);
		idlePos = model.localPosition;
	}


	public override void Activate()
	{
		base.Activate();
		model.localPosition = activatedPos;
		GetComponent<SurgeryToolInput>().enableMovement = false;
	}

	public override void Deactivate()
	{
		base.Deactivate();
		model.localPosition = idlePos;
		GetComponent<SurgeryToolInput>().enableMovement = true;
	}
}
