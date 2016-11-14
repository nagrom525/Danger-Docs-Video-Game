using UnityEngine;
using System.Collections;

public class GauzeSurgeryToolModel : MonoBehaviour {

	public GauzeSurgeryTool gauze;

	void Start()
	{
		gauze = transform.parent.GetComponent<GauzeSurgeryTool>();
	}


	void OnTriggerEnter(Collider other)
	{
		GauzeHotspot hotspot = other.GetComponent<GauzeHotspot>();
		if (hotspot != null)
		{
			hotspot.Activate();
			Debug.Log("touched gauze hotspot");
		}

	}

	void OnTriggerExit(Collider other)
	{
	}
}
