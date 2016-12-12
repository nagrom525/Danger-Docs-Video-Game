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
	public int push_back_threshold = 4;
	private int push_back_num = 0;
    public GameObject actionButtonCanvas;
    public static bool scaredAwayOnce = false;

	public float doctorDashTimerPadding = 1f;   //if all doctors dash the bear within a second of each other

	public Renderer bearRenderer;
	public Material defaultMat;
	public Material hitMat;
	public GameObject bearModel;

	void OnEnable()
	{
		AudioControl.Instance.PlayBearEnter();
	}

	void Awake()
	{
		if (B == null)
		{
			B = this;
		} else 
		{
			Debug.Log("There is more than one bear on the screen");
		}
		bearRenderer = bearModel.GetComponent<Renderer>();
		defaultMat = bearRenderer.sharedMaterial;
	}

	// Use this for initialization
	void Start () {
		AudioControl.Instance.PlayBearEnter();
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
			Debug.Log("bear-doctor collision");
			if (other.gameObject.GetComponent<Doctor>().justDashed)
			{
				push_back_num++;
				bearRenderer.material = hitMat;
				Invoke("ResetMaterial", .2f);
				TutorialEventController.Instance.OnPlayerScaredBear(other.gameObject.GetComponent<DoctorInputController>().playerNum);
			}


			if (push_back_num >= push_back_threshold)
			{
				PatientGurney.parent = null;
				patient.transform.parent = null;
				BearSwitchToCave();
                actionButtonCanvas.SetActive(false);
				this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

			}

			Invoke("ResetThreshold", doctorDashTimerPadding);
		}
		
	}

	void ResetThreshold()
	{
		push_back_num = 0;
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
		AudioControl.Instance.PlayBearExit();
		this.GetComponent<Rigidbody>().velocity = Vector3.zero;
		agent.Stop();
		agent.destination = Cave.transform.position;
		AudioControl.Instance.PlayBearExit();
		agent.Resume();

	}

	void ResetMaterial()
	{
		bearRenderer.material = defaultMat; 
	}

	void BearInCave()
	{
		if (PatientGurney.parent != null)
		{
			PatientGurney.parent = null;
			patient.transform.parent = null;
		}
		AudioControl.Instance.PlayBearExit();
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
 