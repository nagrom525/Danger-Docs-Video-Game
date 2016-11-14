using UnityEngine;
using System.Collections;
using System;

public class GauzeController : MonoBehaviour {

	public GameObject[] gauzeSpots;
	public int gauzeSpotsCount;



	// Use this for initialization
	void Start () {
		gauzeSpotsCount = gauzeSpots.Length;
	}

	public void Soaked()
	{
		gauzeSpotsCount--;
		if (gauzeSpotsCount == 0)
		{
			DoctorEvents.Instance.OnPatientBloodSoaked();
			Destroy(this.gameObject);
		}
	}
}
