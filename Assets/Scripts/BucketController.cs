using UnityEngine;
using System.Collections;

enum BucketState { FULL, EMPTY };

public class BucketController : MonoBehaviour {

	private BucketState bstate;
	private GameObject BucketObject;
	private GameObject water;
	public static BucketController BucketSingleton;

	void Awake()
	{
		if (BucketSingleton == null)
		{
			BucketSingleton = this;
		}
		else
		{
			Debug.Log("Bucket can only be set once");
		}
	}

	// Use this for initialization
	void Start () 
	{
		this.bstate = BucketState.EMPTY;
		BucketObject = this.gameObject;
		water = BucketObject.transform.GetChild(2).gameObject;
		print(water.name);
		water.SetActive(false);

	}
	void FillBucket()
	{
		this.bstate = BucketState.FULL;
		water.SetActive(true);

	}

	BucketState getState()
	{
		return this.bstate;
	}

	void EmptyBucket()
	{
		this.bstate = BucketState.EMPTY;
		water.SetActive(false);
	}
	// Update is called once per frame
	//void Update () {
	
	//}
}
