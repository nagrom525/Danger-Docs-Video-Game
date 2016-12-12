using UnityEngine;
using System.Collections;

public class SutureController : MonoBehaviour {

	public GameObject[] sutures;
	public int sutureCount;
	public int lastPlayerToSuture;
	public void Sitched()
	{
		sutureCount--;
		if (sutureCount == 0)
		{
			DoctorEvents.Instance.OnPatientStitched();
            if (TutorialEventController.Instance.tutorialActive) {
                TutorialEventController.Instance.InformSurgeryComplete(lastPlayerToSuture);
            }
            Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		sutureCount = sutures.Length;
	}

}
