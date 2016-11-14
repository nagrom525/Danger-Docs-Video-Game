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
		Debug.Log("Scalpel tool initiated interaction!!");
	}

	public override void OnDoctorTerminatedInteracting()
	{
		throw new NotImplementedException();
	}
}
