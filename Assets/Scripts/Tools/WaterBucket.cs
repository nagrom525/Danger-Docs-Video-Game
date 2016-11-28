using UnityEngine;
using System.Collections;

public class WaterBucket : Tool {
	public bool hasWater {
		get {return (waterLevel >= 1.0f);}
	}

	private float waterLevel;
	public GameObject water;
	public float splashRadius;
	
	public override ToolType GetToolType() {
		return Tool.ToolType.BUCKET;
	}

	void Start() {
		waterLevel = 0f;
		splashRadius = 4f;
		originalMaterial = transform.GetComponentInChildren<Renderer>().material;

		//water = transform.GetChild(2).gameObject;
		water.SetActive(false);
	}

	public void gainWater(float waterGainRate) {
		print("We've gained water!");
		waterLevel += waterGainRate;
		if (waterLevel > 1.0f) {
			waterLevel = 1.0f;
			water.SetActive(true);
		}
	}

	public void pourWater() {
		waterLevel = 0f;
		water.SetActive(false);
	}


	public override void OnDoctorInitatedInteracting() {
		return;
	}
	public override void OnDoctorTerminatedInteracting() {
		return;
	}


}
