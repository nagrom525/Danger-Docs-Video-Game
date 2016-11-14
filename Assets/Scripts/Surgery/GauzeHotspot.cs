using UnityEngine;
using System.Collections;

public class GauzeHotspot : MonoBehaviour {
	public GauzeController gauzeController;

	public void Activate()
	{
		gauzeController.Soaked();
		Destroy(this.gameObject);
	}
}
