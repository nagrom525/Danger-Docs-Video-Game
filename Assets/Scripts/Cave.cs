using UnityEngine;
using System.Collections;

public class Cave : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag != "Doctor" || coll.gameObject.tag != "Tool")
		{
			Destroy(coll.gameObject);
		}
	}
}
