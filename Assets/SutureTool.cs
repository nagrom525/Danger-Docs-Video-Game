using UnityEngine;
using System.Collections;
using System;

public class SutureTool : Tool {
	public override ToolType GetToolType()
	{
		return ToolType.SUTURE;
	}

	// 
	public override void OnDoctorInitatedInteracting()
	{
		print("OnDoctorInitatedInteracting was called");
	}

	public override void OnDoctorTerminatedInteracting()
	{
		throw new NotImplementedException();
	}
}
