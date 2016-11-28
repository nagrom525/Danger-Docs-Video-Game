using UnityEngine;
using System.Collections;

public class WaterBucket : Tool {
	public bool hasWater {
		get {return (waterLevel >= 1.0f);}
	}

	private float waterLevel;
	public float splashRadius;
	
	public override ToolType GetToolType() {
		return Tool.ToolType.BUCKET;
	}

	void Start() {
		waterLevel = 0f;
		splashRadius = 4f;
		originalMaterial = transform.GetComponentInChildren<Renderer>().material;
	}

	public void gainWater(float waterGainRate) {
		print("We've gained water!");
		waterLevel += waterGainRate;
		if (waterLevel > 1.0f) {
			waterLevel = 1.0f;
		}
	}

	public void pourWater() {
		waterLevel = 0f;
	}


	public override void OnDoctorInitatedInteracting() {
		return;
	}
	public override void OnDoctorTerminatedInteracting() {
		return;
	}


}
