using UnityEngine;
using System.Collections;

public class SutureSurgeryToolModel : MonoBehaviour {

	public SutureSurgeryTool suture;
	public int objectsInTrigger = 0;
	public int playerNumber;
	// Use this for initialization
	void Start()
	{
		suture = transform.parent.GetComponent<SutureSurgeryTool>();
		playerNumber = suture.gameObject.GetComponent<SurgeryToolInput>().playerNum;
	}


	void OnTriggerStay(Collider other)
	{

	}

	void OnTriggerEnter(Collider other)
	{
		SutureHotspot hotspot = other.GetComponent<SutureHotspot>();
		if (hotspot != null)
		{
			hotspot.Activate(playerNumber);
			suture.Deactivate();
		}
	}

	void OnTriggerExit(Collider other)
	{




	}
}
