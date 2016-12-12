﻿using UnityEngine;
using System.Collections;

public class BearAI : MonoBehaviour {

	BearAI B;
	Transform PatientGurney;
	GameObject patient;
	NavMeshAgent agent;
	bool targetAcheived;
	public GameObject Cave;
	Vector3 startposition;
	public int push_back_threshold = 1;
	private int push_back_num;
    public GameObject actionButtonCanvas;
    public static bool scaredAwayOnce = false;

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
        if (scaredAwayOnce) {
            actionButtonCanvas.SetActive(false);
        } else {
            actionButtonCanvas.SetActive(true);
        }
	}

	void OnCollisionEnter(Collision other)
	{
        // bear stealing patient table
		if (other.transform.tag == "PatientTable")
		{
			makeParent(other);
			BearSwitchToCave();
            DoctorEvents.Instance.InformBearStealingPatient();
		}
		else if (other.transform.tag == "Doctor")
		{
			if (push_back_num - 1 <= push_back_threshold && other.gameObject.GetComponent<Doctor>().justDashed)
			{
				PatientGurney.parent = null;
				patient.transform.parent = null;
				BearSwitchToCave();
                actionButtonCanvas.SetActive(false);
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
		//print("trigger anything");
		if (other.transform.tag == "Cave")
		{
			print("why are you not triggering");
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
		AudioControl.Instance.PlayBearExit();
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
 