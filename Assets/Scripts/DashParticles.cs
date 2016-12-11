using UnityEngine;
using System.Collections;

public class DashParticles : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("Destroy", 1f);
	}

	void Destroy()
	{
		Destroy(this.gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
