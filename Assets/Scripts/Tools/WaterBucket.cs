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
	}

	void Update() {
		updateGraphics ();
	}

	public void gainWater(float waterGainRate) {
		waterLevel += waterGainRate;
	}

	public void pourWater() {
		waterLevel = 0f;
	}


	public void updateGraphics () {
		if (hasWater) {
			ps.Play ();
			water.SetActive (true);
		} else {
			ps.Stop ();
			water.SetActive(false);
		}
	}

	public override void OnDoctorInitatedInteracting() {
		return;
	}
	public override void OnDoctorTerminatedInteracting() {
		return;
	}
}
