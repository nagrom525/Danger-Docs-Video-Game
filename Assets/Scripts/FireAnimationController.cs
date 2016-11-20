using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireAnimationController : MonoBehaviour {

	public GameObject fireParticlePrefab;

	public float offsetRange;

	public List<GameObject> fireParticles = new List<GameObject>();

	void CreateFire()
	{
		GameObject f = (GameObject)Instantiate(fireParticlePrefab);
		f.transform.parent = this.transform;
		f.transform.localPosition = Vector3.zero;
		OffsetParticle(f.transform);
		fireParticles.Add(f);


	}

	void OffsetParticle(Transform t)
	{
		Vector3 origin = t.localPosition;
		origin += new Vector3(Random.Range(-offsetRange, offsetRange), 0f, Random.Range(-offsetRange, offsetRange));
		t.localPosition = origin;
	}
		

	// Use this for initialization
	void Start () {
		InvokeRepeating("CreateFire", .2f, .2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
