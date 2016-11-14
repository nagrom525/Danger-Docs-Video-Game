using UnityEngine;
using System.Collections;
using System;

public class ScalpelTool : Tool {
	public override ToolType GetToolType()
	{
		return ToolType.SCALPEL;
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
