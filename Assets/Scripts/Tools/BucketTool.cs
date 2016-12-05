using UnityEngine;
using System.Collections;
using System;

//don't know what to do with this because you can't use it on the patient

public class BucketTool : Tool {
	public override ToolType GetToolType()
	{
		return ToolType.BUCKET;
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
