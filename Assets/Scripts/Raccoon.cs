using UnityEngine;
using System.Collections;

public class Raccoon : MonoBehaviour {

	public enum RaccoonState
	{
		Searching,
		HoldingPickup,
		Leaving
	};

	public float 	pickupSpeed;
	public float 	idleSpeed;
	public float 	leavingSpeed;

	public Transform 		pickupAnchor;
	public Transform 		dropAnchor;
	public GameObject 		closestPickup;

	public Tool 			currentPickup;

	public RaccoonState currentState;


	// Use this for initialization
	void Start () {
		currentState = RaccoonState.Searching;


		GetNearestTool();
	}

	public void GetNearestTool()
	{
		Transform tMin = null;
		float minDist = Mathf.Infinity;
		Vector3 currentPos = transform.position;

		GameObject[] tools = GameObject.FindGameObjectsWithTag("Tool");
			
		foreach (GameObject t in tools)
		{
			float dist = Vector3.Distance(t.transform.position, currentPos);
			if (dist < minDist)
			{
				tMin = t.transform;
				minDist = dist;
			}
		}
		closestPickup = tMin.gameObject;
	
	}


	// Update is called once per frame
	void Update () {
		switch (currentState)
		{
			case RaccoonState.Searching:
				Searching();
				break;
			case RaccoonState.HoldingPickup:
				HoldingPickup();
				break;
			case RaccoonState.Leaving:
				Leaving();
				break;
		}
	}

	void PickupTool(Tool tool)
	{
		tool.enabled = false;
		tool.transform.position = pickupAnchor.position;
		tool.gameObject.transform.parent = this.gameObject.transform;
		currentPickup = tool;
	}

	void DropTool()
	{
		if (currentPickup)
		{
			currentPickup.transform.parent = null;
			currentPickup.transform.position = dropAnchor.position;
			currentPickup.enabled = true;
			currentPickup = null;
		}
	}

	void Searching()
	{
		//get nearest pickup

		//look at it
		transform.LookAt(closestPickup.transform);
		//move to it
		transform.position = Vector3.Lerp(transform.position, closestPickup.transform.position,pickupSpeed*Time.deltaTime);

		//when in range, switch state to holding pickup
		if (Vector3.Distance(transform.position, closestPickup.transform.position) < 2f)
		{
			currentState = RaccoonState.HoldingPickup;
		}

	}
	void HoldingPickup()
	{
		//run to a point away from Doctors
		//

	}
	void Leaving()
	{
		//
	}
}

