using UnityEngine;
using System.Collections;

public class TutorialPatientCameras : MonoBehaviour {
	public GameObject[] cameras;
	public LevelUserInterface lvlUI;
	void Start () {
		TutorialEventController.Instance.OnPickupToolsEnd += ShowCameras;
		TutorialEventController.Instance.OnSurgeryOnPatientStart += ShowCameras;
		TutorialEventController.Instance.OnSurgeryOnPatientEnd += HideCameras;

	}

	void ShowCameras()
	{
		lvlUI.SetHeartMonitorActive(false);
		foreach (GameObject x in cameras)
			x.SetActive(true);
	}

	void HideCameras()
	{
		lvlUI.SetHeartMonitorActive(true);
		foreach (GameObject x in cameras)
			x.SetActive(false);
	}

	/*
	void OnEnable()
	{
		lvlUI.SetHeartMonitorActive(false);
	}
	*/

	void OnDisable()
	{
		lvlUI.SetHeartMonitorActive(true);
	}

}
