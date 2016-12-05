using UnityEngine;
using System.Collections;

public class WaterBucket : Tool
{

	public ParticleSystem ps;
	public bool hasWater
	{
		get { return (waterLevel >= 1.0f); }
	}

	private float waterLevel;
	public GameObject water;
	public float splashRadius;
	public GameObject puddlePrefab;

	public override ToolType GetToolType()
	{
		return Tool.ToolType.BUCKET;
	}

	void Start()
	{
		waterLevel = 0f;
		splashRadius = 5f;
		originalMaterial = transform.GetComponentInChildren<Renderer>().material;
		ps = transform.GetComponentInChildren<ParticleSystem>();
	}

	void Update()
	{
		updateGraphics();
	}

	public void gainWater(float waterGainRate)
	{
		waterLevel = (waterLevel + waterGainRate < 1f) ? (waterLevel + waterGainRate) : 1f;
		if (waterLevel > 1f) waterLevel = 1f;
	}

	public void pourWater(Vector3 docDirection)
	{
		waterLevel = 0f;
		Vector3 puddlePos = new Vector3(transform.position.x, 0f, transform.position.z) + (docDirection * 4f);
		GameObject go = (GameObject)Instantiate(puddlePrefab, puddlePos, Quaternion.identity);
		go.transform.localEulerAngles = new Vector3(0f, Random.Range(0, 360), 0f);
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
