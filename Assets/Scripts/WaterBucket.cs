using UnityEngine;
using System.Collections;

public class WaterBucket : Tool {
	public bool hasWater {
		get {return (waterLevel >= 1.0f);}
	}

	private float waterLevel;
	private float waterGainRate;	
	
	public override ToolType GetToolType() {
		return Tool.ToolType.BUCKET;
	}

	void Start() {
		waterLevel = 0f;
		waterGainRate = 0.25f;
		originalMaterial = transform.GetComponentInChildren<Renderer>().material;
	}

	public void gainWater() {
		print("We've gained water!");
		waterLevel += waterGainRate * Time.deltaTime;
		if (waterLevel > 1.0f) {
			waterLevel = 1.0f;
		}
	}


	public override void OnDoctorInitatedInteracting() {
		return;
	}
	public override void OnDoctorTerminatedInteracting() {
		return;
	}


}
