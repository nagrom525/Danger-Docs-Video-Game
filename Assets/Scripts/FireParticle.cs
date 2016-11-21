using UnityEngine;
using System.Collections;

public class FireParticle : MonoBehaviour {

	public float 		speedMax = .2f;
	public float 		shrinkRate = .7f;
	private float 		spd;
	public FireAnimationController fac;

	// Use this for initialization
	void Start () {
		float rand = Random.Range(.5f, 4f);
		transform.localScale = new Vector3(rand, rand, rand);

		spd = Random.Range(.1f,speedMax);
		fac = transform.parent.GetComponent<FireAnimationController>();
	}

	void LateUpdate () {
		//move up, get smaller, destroy when small enough
		Vector3 newPos = transform.localPosition + new Vector3(0f,spd, 0f);
		transform.localPosition = newPos;
		Vector3 newScale = transform.localScale *= shrinkRate;
		transform.localScale = newScale;

		if (transform.localScale.x < .4f)
		{
			fac.fireParticles.Remove(this.gameObject);
			Destroy(gameObject);
		}
	}
}
