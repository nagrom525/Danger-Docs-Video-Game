using UnityEngine;
using System.Collections;

public class WaterPuddle : MonoBehaviour {

	public bool shrinking;
	public float shrinkRate;
	private Vector3 scale;

	// Use this for initialization
	void Start () {
		transform.localScale = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if (!shrinking)
		{
			scale = transform.localScale + new Vector3(shrinkRate, shrinkRate, shrinkRate);
			transform.localScale = scale;
			if (transform.localScale.x > 1f)
				shrinking = true;
		}
		else
		{
			scale = transform.localScale - new Vector3(shrinkRate/2, shrinkRate/2, shrinkRate/2);
			transform.localScale = scale;
			if (transform.localScale.x < .05f)
			{
				Destroy(this.gameObject);
			}
		}

	}
}
