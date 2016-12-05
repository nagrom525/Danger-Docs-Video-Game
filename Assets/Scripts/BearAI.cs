using UnityEngine;
using System.Collections;

public class BearAI : MonoBehaviour {

	BearAI B;
	Transform PatientGurney;
	GameObject patient;
	NavMeshAgent agent;
	bool targetAcheived;
	public GameObject Cave;
	Vector3 startposition;

	void Awake()
	{
		if (B == null)
		{
			B = this;
		} else 
		{
			Debug.Log("There is more than one bear on the screen");
		}
	}

	// Use this for initialization
	void Start () {
		patient = Patient.Instance.gameObject;
		this.agent = GetComponent<NavMeshAgent>();
		this.agent.destination = patient.transform.position;
		this.transform.LookAt(patient.transform.position);
		//targetAcheived = false;
		startposition = this.gameObject.transform.position;
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.transform.tag == "PatientTable")
		{
			makeParent(other);
			BearSwitchToCave();
		}
		else if (other.transform.tag == "Doctor")
		{
			//if(other.gameObject.GetComponent<Doctor> ()
		}
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Cave")
		{
			BearInCave();
			this.gameObject.SetActive(false);
			this.gameObject.transform.position = startposition;
		}
	}

	void makeParent(Collision other)
	{
		other.gameObject.transform.parent.parent = this.transform;
		print("transform parent " + other.gameObject.transform.parent.name);
		PatientGurney = other.transform.parent;
		patient.transform.parent = this.transform;
	}

	void BearSwitchToCave()
	{
		this.GetComponent<Rigidbody>().velocity = Vector3.zero;
		agent.Stop();
		agent.destination = Cave.transform.position;
		agent.Resume();

	}

	void BearInCave()
	{
		PatientGurney.parent = null;
		patient.transform.parent = null;
		DoctorEvents.Instance.InformBearLeft();

	}

	// Update is called once per frame
	//void Update()
	//{
	//	this.GetComponent<Rigidbody>().velocity = Vector3.zero;
	//	//agent.destination = patient.transform.position;
	//	//if(targetAcheived) agent.Stop();
	//}

}
 