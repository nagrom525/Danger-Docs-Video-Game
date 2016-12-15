using UnityEngine;
using System.Collections;

public class WaterBucket : Tool
{

	public ParticleSystem ps;
	public bool hasWater
	{
		get { return (waterLevel >= 1f - Mathf.Epsilon); }
	}

	private float waterLevel;
	public GameObject water;
	public float splashRadius;
	public GameObject puddlePrefab;
    public GameObject actionButtonCanvas;
    private bool fire = false;
    private bool bucketFilledOnce = false;
    private bool bucketHeldTutorial = false;

	public override ToolType GetToolType()
	{
		return ToolType.BUCKET;
	}

	void Start()
	{
		waterLevel = 0f;
		splashRadius = 7f;
		originalMaterial = transform.GetComponentInChildren<Renderer>().material;
		ps = transform.GetComponentInChildren<ParticleSystem>();
        DoctorEvents.Instance.onFire += OnFire;
        TutorialEventController.Instance.OnFireStart += OnFireStartTutorial;
        TutorialEventController.Instance.OnFirePutOut += OnfireEndTutorial;
        TutorialEventController.Instance.OnToolPickedUp += OnToolPickedUpTutorial;
        TutorialEventController.Instance.OnToolDropped += OnToolDroppedTutorial;
        DoctorEvents.Instance.onBucketPickedUp += OnBucketPickedUp;
        DoctorEvents.Instance.onBucketDropped += OnBucketDropped;
        DoctorEvents.Instance.onBucketFilled += OnBucketFilled;
        DoctorEvents.Instance.onFirePutOut += OnFirePutOut;
        updateGraphics();
	}


	public void gainWater(float waterGainRate)
	{
		waterLevel = (waterLevel + waterGainRate < 1f) ? (waterLevel + waterGainRate) : 1f;
		if (waterLevel > 1f) waterLevel = 1f;
		AudioControl.Instance.PlayWaterBucketFill();
        updateGraphics();
	}

	public void pourWater(Vector3 docDirection)
	{
		waterLevel = 0f;
		Vector3 puddlePos = new Vector3(transform.position.x, 0f, transform.position.z) + (docDirection * 4f);
		GameObject go = (GameObject)Instantiate(puddlePrefab, puddlePos, Quaternion.identity);
		go.transform.localEulerAngles = new Vector3(0f, Random.Range(0, 360), 0f);
        DoctorEvents.Instance.InformBucketEmptied();
        updateGraphics();
		AudioControl.Instance.PlayWaterBucketDump();
	}


	public void updateGraphics()
	{
		if (hasWater)
		{
			ps.Play();
			water.SetActive(true);
		}
		else {
			ps.Stop();
			water.SetActive(false);
		}
	}

	public override void OnDoctorInitatedInteracting()
	{
		return;
	}
	public override void OnDoctorTerminatedInteracting()
	{
		return;
	}

    private void OnFire(float duation) {
        fire = true;
        if (!bucketFilledOnce) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        }
    }

    private void OnFireStartTutorial() {
        fire = true;
        if (!bucketHeldTutorial) {
            actionButtonCanvas.SetActive(true);
        }
        
    }

    private void OnfireEndTutorial(int playerNum) {
        fire = false;
        actionButtonCanvas.SetActive(false);
    }

    private void OnFirePutOut(float duration) {
        fire = false;
    }

    private void OnBucketPickedUp(bool full) {
        actionButtonCanvas.SetActive(false);
    }

    private void OnBucketDropped(bool full) {
        if (fire && !bucketFilledOnce) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        }
    }

    private void OnBucketFilled(float duration) {
        if (!TutorialEventController.Instance.tutorialActive) {
            bucketFilledOnce = true;
        }
    }

    private void OnFireTutorial() {
        OnFire(0.0f);
    }



    private void OnToolPickedUpTutorial(Tool.ToolType type, int playerNum) {
        if(type == ToolType.BUCKET) {
            bucketHeldTutorial = true;
        }

    }

    private void OnToolDroppedTutorial(Tool.ToolType type, int playerNum) {
        if(type == ToolType.BUCKET) {
            bucketHeldTutorial = false;
        }
    }

    void OnDestroy() {
        DoctorEvents.Instance.onFire -= OnFire;
        DoctorEvents.Instance.onBucketPickedUp -= OnBucketPickedUp;
        DoctorEvents.Instance.onBucketDropped -= OnBucketDropped;
        DoctorEvents.Instance.onBucketFilled -= OnBucketFilled;
        DoctorEvents.Instance.onFirePutOut -= OnFirePutOut;
        TutorialEventController.Instance.OnFireStart -= OnFireStartTutorial;
        TutorialEventController.Instance.OnFirePutOut -= OnfireEndTutorial;
        TutorialEventController.Instance.OnToolPickedUp -= OnToolPickedUpTutorial;
        TutorialEventController.Instance.OnToolDropped -= OnToolDroppedTutorial;
    }
}
