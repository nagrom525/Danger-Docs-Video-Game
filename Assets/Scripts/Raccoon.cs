using UnityEngine;
using System.Collections;

public class Raccoon : MonoBehaviour {

	public enum RaccoonState
	{
		Searching,
		HoldingPickup,
		Leaving
	};

	public float 	searchingSpeed;
	public float 	pickupSpeed;
	public float 	leavingSpeed;

	public Vector3 leavingTarget;

	public Transform 		pickupAnchor;
	public Transform 		dropAnchor;
	public GameObject 		closestPickup;

	public Tool 			currentPickup;

	public RaccoonState currentState;


	// Use this for initialization
	void Start () {
		currentState = RaccoonState.Searching;
		GetNearestTool();
		leavingTarget = transform.position;
		InvokeRepeating("GetNearestTool", .1f, .2f);
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
			if (dist < minDist && t.transform.parent == null)
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

		if (tool.transform.parent == null)
		{
			tool.enabled = false;
			tool.transform.position = pickupAnchor.position;
			tool.gameObject.transform.parent = this.gameObject.transform;
			if (tool.GetComponent<Rigidbody>())
				tool.GetComponent<Rigidbody>().isKinematic = true;
			currentPickup = tool;
		}

	}

	void DropTool()
	{
		if (currentPickup)
		{
			currentPickup.transform.parent = null;
			currentPickup.transform.position = dropAnchor.position;
			currentPickup.enabled = true;
			if (currentPickup.GetComponent<Rigidbody>())
				currentPickup.GetComponent<Rigidbody>().isKinematic = false;
			currentPickup = null;


		}
	}

	void Searching()
	{
		if (closestPickup)
		{
			//look at it
			transform.LookAt(closestPickup.transform);
			//move to it
			transform.position = Vector3.Lerp(transform.position, closestPickup.transform.position, searchingSpeed * Time.deltaTime);

			//when in range, switch state to holding pickup
			if (Vector3.Distance(transform.position, closestPickup.transform.position) < 2f)
			{
				Debug.Log("Raccoon is picking up tool!");
				PickupTool(closestPickup.GetComponent<Tool>());
				currentState = RaccoonState.HoldingPickup;
			}
		}
		else
		{
			currentState = RaccoonState.Leaving;
		}
	}

	void HoldingPickup()
	{
		//run to a point away from Doctors
		if (currentPickup == null)
		{
			currentState = RaccoonState.Searching;
		}
		//look at it
		transform.LookAt(leavingTarget);
		//move to it
		transform.position = Vector3.Lerp(transform.position, leavingTarget, pickupSpeed * Time.deltaTime);

		if (currentPickup)
			if (currentPickup.transform.parent != this.transform)
			{
				//pickup was taken by doctor, leave
				currentState = RaccoonState.Leaving;
			}

		if (Vector3.Distance(transform.position, leavingTarget) < 3.5f)
		{
			//notify event manager that tool was stolen
			DoctorEvents.Instance.InformToolTaken(currentPickup);
			//destroy self
			Destroy(this.gameObject);
		}

	}
	void Leaving()
	{
		//look at it
		transform.LookAt(leavingTarget);
		//move to it
		transform.position = Vector3.Lerp(transform.position, leavingTarget, leavingSpeed * Time.deltaTime);

		if (Vector3.Distance(transform.position, leavingTarget) < 3.5f)
		{
			//notify event manager that tool was stolen
			//destroy self
			Destroy(this.gameObject);
		}
	}
}

