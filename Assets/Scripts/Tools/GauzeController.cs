using UnityEngine;
using System.Collections;
using System;

public class GauzeController : MonoBehaviour {

	public GameObject[] gauzeSpots;
	public int gauzeSpotsCount;

	public int lastPlayerToSoak;


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
			TutorialEventController.Instance.OnSurgeryComplete(lastPlayerToSoak);
			foreach (GameObject go in gauzeSpots)
			{
				go.GetComponent<GauzeHotspot>().Reset();
			}
			Destroy(this.gameObject);
		}
	}
}
