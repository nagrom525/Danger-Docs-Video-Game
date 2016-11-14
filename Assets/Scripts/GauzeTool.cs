using UnityEngine;
using System.Collections;
using System;

public class GauzeTool : Tool {
	public override ToolType GetToolType()
	{
		return ToolType.GAUZE;
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
