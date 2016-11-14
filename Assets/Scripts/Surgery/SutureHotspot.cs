using UnityEngine;
using System.Collections;

public class SutureHotspot : MonoBehaviour {

	public SutureController sutureController;

	public void Activate()
	{
		sutureController.Sitched();
		Destroy(this.gameObject);
	}
}
