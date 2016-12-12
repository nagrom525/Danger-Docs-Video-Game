using UnityEngine;
using System.Collections;

public class GauzeSurgeryToolModel : MonoBehaviour {

	public GauzeSurgeryTool gauze;
	public int playerNumber;

	void Start()
	{
		gauze = transform.parent.GetComponent<GauzeSurgeryTool>();
		playerNumber = gauze.gameObject.GetComponent<SurgeryToolInput>().playerNum;
	}


	void OnTriggerEnter(Collider other)
	{
		GauzeHotspot hotspot = other.GetComponent<GauzeHotspot>();
		if (hotspot != null)
		{
			hotspot.Activate(playerNumber);

			Debug.Log("touched gauze hotspot");
		}

	}

	void OnTriggerExit(Collider other)
	{
	}
}
