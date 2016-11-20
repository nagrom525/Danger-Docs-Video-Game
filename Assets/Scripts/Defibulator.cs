using UnityEngine;
using System.Collections;
using System;

public class Defibulator : Tool
{
    public override ToolType GetToolType()
    {
        return ToolType.DEFIBULATOR;
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
