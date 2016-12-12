using UnityEngine;
using System.Collections;

public class TutorialPatientCameras : MonoBehaviour {
	public GameObject[] cameras;

	void Start () {
		TutorialEventController.Instance.OnPickupToolsEnd += ShowCameras;
		TutorialEventController.Instance.OnSurgeryOnPatientStart += ShowCameras;
		TutorialEventController.Instance.OnSurgeryOnPatientEnd += HideCameras;

	}

	void ShowCameras()
	{
		foreach (GameObject x in cameras)
			x.SetActive(true);
	}

	void HideCameras()
	{
		foreach (GameObject x in cameras)
			x.SetActive(false);
	}

}
