using UnityEngine;
using System.Collections;

public class ScalpelSurgeryToolModel : MonoBehaviour {

	public ScalpelSurgeryTool scalpel;
	public int objectsInTrigger = 0;

	// Use this for initialization
	void Start()
	{
		scalpel = transform.parent.GetComponent<ScalpelSurgeryTool>();
	}


	void OnTriggerStay(Collider other)
	{
		ScalpelTrack track = other.GetComponent<ScalpelTrack>();
		if (track != null)
		{
			Debug.Log("Scalpel on track.");
			//if start
			track.Activate();
			//if end
			if (track.isStart)
				scalpel.touchedStart = true;

			if (track.isEnd)
				scalpel.touchedEnd = true;

			if (track.isMidTrack)
				scalpel.inMidTrack = true;
			//otherwise middle
		}

	}

	void OnTriggerEnter(Collider other)
	{
		objectsInTrigger++;
	}

	void OnTriggerExit(Collider other)
	{
		//if neither start or end
		//hurt patient
		objectsInTrigger--;


		ScalpelTrack track = other.GetComponent<ScalpelTrack>();
		if (track != null)
		{
			//if start
			track.Deactivate();
			//if end
			if (track.isStart && scalpel.inMidTrack)
			{
				return;
			}


			if (scalpel.touchedStart && scalpel.touchedEnd)
			{
				//succesful procedure
				Debug.Log("Succesful procedure!");

				DoctorEvents.Instance.OnPatientCutOpen();
         

                if (TutorialEventController.Instance.tutorialActive) {
                    TutorialEventController.Instance.InformSurgeryComplete(scalpel.gameObject.GetComponent<SurgeryToolInput>().playerNum);
                }

                Destroy(track.transform.parent.gameObject);
			}
			else if (objectsInTrigger == 0)
			{
				//do nothing
				scalpel.Deactivate();
			}

		}
	}
}
