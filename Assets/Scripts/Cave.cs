using UnityEngine;
using System.Collections;

public class Cave : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag != "Doctor")
		{

		}
		else if (coll.gameObject.tag != "Tool")
		{
		} else {
			Destroy(coll.gameObject);
		}
	}
}
