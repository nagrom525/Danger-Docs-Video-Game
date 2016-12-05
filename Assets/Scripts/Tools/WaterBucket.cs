using UnityEngine;
using System.Collections;

public class WaterBucket : Tool
{

	public ParticleSystem ps;
	public bool hasWater
	{
		get { return (waterLevel >= 1f - Mathf.Epsilon); }
	}

	private float waterLevel;
	public GameObject water;
	public float splashRadius;
	public GameObject puddlePrefab;

	public override ToolType GetToolType()
	{
		return ToolType.BUCKET;
	}

	void Start()
	{
		waterLevel = 0f;
		splashRadius = 7f;
		originalMaterial = transform.GetComponentInChildren<Renderer>().material;
		ps = transform.GetComponentInChildren<ParticleSystem>();
		updateGraphics();
	}


	public void gainWater(float waterGainRate)
	{
		waterLevel += waterGainRate;
		waterLevel = Mathf.Clamp(waterLevel, 0f, 1f);
		updateGraphics();
	}

	public void pourWater(Vector3 docDirection)
	{
		waterLevel = 0f;
		Vector3 puddlePos = new Vector3(transform.position.x, 0f, transform.position.z) + (docDirection * 4f);
		GameObject go = (GameObject)Instantiate(puddlePrefab, puddlePos, Quaternion.identity);
		go.transform.localEulerAngles = new Vector3(0f, Random.Range(0, 360), 0f);
		updateGraphics();
	}


	public void updateGraphics()
	{
		if (hasWater)
		{
			ps.Play();
			water.SetActive(true);
		}
		else {
			ps.Stop();
			water.SetActive(false);
		}
	}

	public override void OnDoctorInitatedInteracting()
	{
		return;
	}
	public override void OnDoctorTerminatedInteracting()
	{
		return;
	}
}
