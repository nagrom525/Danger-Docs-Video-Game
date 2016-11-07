using UnityEngine;
using System.Collections;
using System;

public class OrangeDefibulator : Tool
{
    public override ToolType GetToolType()
    {
        return ToolType.TYPE_4;
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
