using UnityEngine;
using System.Collections;

public class WaterBucket : Tool {

	public ParticleSystem ps;
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
		splashRadius = 5f;
		originalMaterial = transform.GetComponentInChildren<Renderer>().material;
		ps = transform.GetComponentInChildren<ParticleSystem> ();
		ps.Stop ();

		//water = transform.GetChild(2).gameObject;
		water.SetActive(false);
	}

	public void gainWater(float waterGainRate) {
		print("We've gained water!");
		waterLevel += waterGainRate;
		if (waterLevel > 1.0f) {
			waterLevel = 1.0f;
			ps.Play ();
			water.SetActive(true);
		}
	}

	public void pourWater() {
		waterLevel = 0f;
		ps.Stop ();
		water.SetActive(false);
	}


	public override void OnDoctorInitatedInteracting() {
		return;
	}
	public override void OnDoctorTerminatedInteracting() {
		return;
	}
}
