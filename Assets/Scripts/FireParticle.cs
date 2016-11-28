using UnityEngine;
using System.Collections;

public class FireParticle : MonoBehaviour {

	public float 					speedMax = .2f;
	public float 					shrinkRate = .7f;
	private float 					spd;
	public FireAnimationController 	fac;
	public bool 					shrink = false;
	private float 					rand;
	private Renderer 				rend;

	// Use this for initialization
	void Start () {
		rand = Random.Range(.2f, 2f);
		rend = GetComponent<Renderer>();
		transform.localScale = Vector3.zero;
		transform.localScale += new Vector3(rand/10f, rand/10f, rand/10f);

		transform.localEulerAngles = new Vector3(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360));

		spd = Random.Range(.1f,speedMax);
		fac = transform.parent.GetComponent<FireAnimationController>();
	}

	void LateUpdate () {
		//move up, get smaller, destroy when small enough
		Vector3 newPos = transform.localPosition + new Vector3(0f,spd, 0f);
		transform.localPosition = newPos;



		if (shrink)
		{
			Vector3 newScale = transform.localScale *= shrinkRate;
			transform.localScale = newScale;

			if (transform.localScale.x < 3f)
				rend.material = fac.GetHotColor();
			if (transform.localScale.x < 2f)
				rend.material = fac.GetHotterColor();
			if (transform.localScale.x < 1f)
				rend.material = fac.GetHottestColor();

			//Destroy sequence
			if (transform.localScale.x < .5f)
			{
				fac.fireParticles.Remove(this.gameObject);
				Destroy(gameObject);
			}
		}
		else
		{
			//grow
			Vector3 newScale = transform.localScale *= 1.2f;
			transform.localScale = newScale;
			if (transform.localScale.x > rand)
			{
				shrink = true;
			}
		}


	}
}
