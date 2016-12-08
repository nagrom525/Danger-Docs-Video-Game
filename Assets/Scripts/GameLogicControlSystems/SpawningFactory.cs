using UnityEngine;
using System.Collections;

public class SpawningFactory : MonoBehaviour {
    public GameObject bearPrefab;
    public GameObject firePrefab;
    public GameObject raccoonPrefab;

    // -- Fire values -- //
    public float probOfFire = 0.05f;
    public float minimumDelayBetweenFires = 5.0f; 
    private bool fire = false; // if there is currently a fire
    private float lastFire = 0.0f;
    public bool fireActive = false;
    public float timeForFistFire = 30.0f;


    // -- bearValues -- //
    public float probOfBear = 0.05f;
    public float minimumDelayBetweenBears = 5.0f;
    private bool bear = false; // if there is currently a bear
    private float lastBear = 0.0f;
    public bool bearActive = false;
    public float timeForFirstBear = 40.0f;


    // raccoonValues -- //
    public float probOfRaccoon = 0.05f;
    public float minimumDelayBetweenRaccoons = 4.0f;
    private float lastRaccoon = 0.0f;
    public bool raccoonActive = false;
    public float timeForFirstRaccoon = 20.0f;

    // -- general values --//
    private float lastSecond = 0.0f;


	public GameObject bearObj;

	// Use this for initialization
	void Start () {
        DoctorEvents.Instance.onFirePutOut += OnFirePutOut;
        DoctorEvents.Instance.onBearLeft += OnBearLeft;
        lastSecond = Time.time;
        lastBear = Time.time;
        lastRaccoon = Time.time;
        lastFire = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        bool timeToCheck = TimeToCheck();
        FireUpdate(timeToCheck);
        BearUpdate(timeToCheck);
        RaccoonUpdate(timeToCheck);
	
	}

    private void FireUpdate(bool timeToCheck) {
        if (timeToCheck && !fire && fireActive &&
            ((Time.time - lastFire) > minimumDelayBetweenFires) && ShouldSpawn(probOfFire)) {
            DoctorEvents.Instance.InformFire(0);
            fire = true;
            // need to have some way of deciding where to instiantiate the fire!
            Flame flame = (Instantiate(firePrefab) as GameObject).GetComponent<Flame>();
            flame.motherFlame = true;
        } else if(!fireActive && (Time.time - lastFire) >= timeForFistFire) {
            DoctorEvents.Instance.InformFire(0);
            fire = true;
            fireActive = true;
            // need to have some way of deciding where to instiantiate the fire!
            Flame flame = (Instantiate(firePrefab) as GameObject).GetComponent<Flame>();
            flame.motherFlame = true;
        }
    }

    private void BearUpdate(bool timeToCheck) {
        if (timeToCheck && bearActive && !bear &&
            ((Time.time - lastBear) > minimumDelayBetweenBears) && ShouldSpawn(probOfBear)) {
            DoctorEvents.Instance.InformBearAttack(0);
            bear = true;
			bearObj.SetActive(true);
        } else if (!bearActive &&  (Time.time - lastBear) >= timeForFirstBear) {
            DoctorEvents.Instance.InformBearAttack(0);
            bear = true;
            bearActive = true;
            bearObj.SetActive(true);
        }
    }

   private void RaccoonUpdate(bool timeToCheck) {
        if (timeToCheck && raccoonActive && ((Time.time - lastRaccoon) > minimumDelayBetweenRaccoons) && ShouldSpawn(probOfRaccoon)) {
            DoctorEvents.Instance.InformRacconAttack(0);
            lastRaccoon = Time.time;
			// raccon spawning code
			SpawnRaccoon();
			SpawnRaccoon();
			//SpawnRaccoon();

        } else if(!raccoonActive && (Time.time - lastRaccoon) >= timeForFirstRaccoon) {
            lastRaccoon = Time.time;
            DoctorEvents.Instance.InformRacconAttack(0);
            SpawnRaccoon();
            SpawnRaccoon();
            raccoonActive = true;
        }
    }

	void SpawnRaccoon() {
		float ang = Random.value * 360;
		Vector3 pos = new Vector3(0f,0f,0f);
		float radius = 45f;
		pos.x = radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.z = radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		GameObject coon = (GameObject)Instantiate(raccoonPrefab);
		coon.transform.position = pos;
	}

    private void OnFirePutOut(float duration) {
        fire = false;
        lastFire = Time.time;
    }

    private void OnBearLeft(float duration) {
        bear = false;
        lastBear = Time.time;
    }

    // checks to see if a second has passed since the last check
    // if a second has passed returns true, and changes the "lastSecond"
    // memeber to the current time
    private bool TimeToCheck() {
        if((Time.time - lastSecond) > 1.0f) {
            lastSecond = Time.time;
            return true;
        }
        return false;
    }

    private bool ShouldSpawn(float probability) {
        if(Random.value < probability) {
            return true;
        }
        return false;
    }
}
