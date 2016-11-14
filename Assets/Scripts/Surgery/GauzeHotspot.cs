using UnityEngine;
using System.Collections;

public class GauzeHotspot : MonoBehaviour {
	public GauzeController gauzeController;

	public void Activate()
	{
		gauzeController.Soaked();
		GetComponent<Renderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
	}

	public void Reset()
	{
		GetComponent<Renderer>().enabled = true;
		GetComponent<Collider>().enabled = true;
	}
}
