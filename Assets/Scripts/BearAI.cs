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
	public int push_back_threshold = 3;
	private int push_back_num;

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
			if (push_back_num - 1 <= push_back_threshold && other.gameObject.GetComponent<Doctor>().justDashed)
			{
				PatientGurney.parent = null;
				patient.transform.parent = null;
				BearSwitchToCave();
				this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
			else if (other.gameObject.GetComponent<Doctor>().justDashed)
			{
				push_back_num--;

			}
			else {
				//stun person
			}
		}
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Cave")
		{
			this.gameObject.SetActive(false);
			if (patient.transform.parent != null)
			{
				patient.SetActive(false);
				DoctorEvents.Instance.InducePatientDeath();
			}
			BearInCave();
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
		if (PatientGurney.parent != null)
		{
			PatientGurney.parent = null;
			patient.transform.parent = null;
		}
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
 