using UnityEngine;
using System.Collections;
using System;

public class BlueDefibulator : Tool
{
    public override ToolType GetToolType()
    {
        return ToolType.TYPE_1;
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
