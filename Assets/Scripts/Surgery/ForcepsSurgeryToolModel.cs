using UnityEngine;
using System.Collections;

public class ForcepsSurgeryToolModel : MonoBehaviour {
	
	public ForcepsSurgeryTool forceps;

	void Start()
	{
		forceps = transform.parent.GetComponent<ForcepsSurgeryTool>();
	}


	void OnTriggerEnter(Collider other)
	{
		ForcepsHotspot hotspot = other.GetComponent<ForcepsHotspot>();
		if (hotspot != null)
			forceps.openBody = true;
	}

	void OnTriggerExit(Collider other)
	{
		forceps.openBody = false;
	}
}
