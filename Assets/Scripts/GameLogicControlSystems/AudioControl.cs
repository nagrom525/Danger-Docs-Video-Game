using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioControl : MonoBehaviour {


    public AudioClip heartMonitorBeep;
    public AudioClip heartMonitorLong;

	public AudioClip bearEnter;

	public AudioClip bearExit;	//when bear is succesfully scared away

	public AudioClip toolPickup;
	public AudioClip toolDrop;

	public AudioClip defibulatorSurge;	//plays when defibulator is used

	public AudioClip doctorDash;
	public AudioClip doctorBump;

	public AudioClip surgeryEnter;
	public AudioClip surgeryExit;
	public AudioClip surgeryInteract;

	public AudioClip waterBucketDump;
	public AudioClip waterBucketFill;

	public AudioClip fireLoop;  //looping fire when a fire has started

	public AudioClip anestheticMachineFill; //when a battery is used to fill up the machine

	public AudioClip raccoonSteal;  //plays when raccoon picks up an item
	public AudioClip operationSuccessful;
	public AudioClip tutorialTaskComplete;

	public AudioClip tutorialBeginMusic;
	public AudioClip tutorialLoopMusic;
	public AudioClip dashCharge;
	public AudioClip playAddTutorialCircle;

    private AudioSource mainGameMusicAudioSrc;
	private AudioSource heartRateAudioSrc;
	private AudioSource bearEnterAudioSrc;
	private AudioSource bearExitAudioSrc;
	private AudioSource toolPickupAudioSrc;
	private AudioSource toolDropAudioSrc;
	private AudioSource defibulatorSurgeAudioSrc;
	private AudioSource doctorDashAudioSrc;
	private AudioSource doctorBumpAudioSrc;
	private AudioSource surgeryEnterAudioSrc;
	private AudioSource surgeryExitAudioSrc;
	private AudioSource surgeryInteractAudioSrc;
	private AudioSource waterBucketDumpAudioSrc;
	private AudioSource waterBucketFillAudioSrc;
	private AudioSource fireLoopAudioSrc;
	private AudioSource anestheticMachineFillAudioSrc;
	private AudioSource raccoonStealAudioSrc;
	private AudioSource operationSuccessfulAudioSrc;
	private AudioSource tutorialTaskCompleteAudioSrc;
	private AudioSource tutorialBeginMusicSrc;
	private AudioSource tutorialLoopMusicSrc;
	private AudioSource dashChargeSrc;
	private AudioSource playAddTutorialCircleSrc;

    private static AudioControl _instance;
    public static AudioControl Instance {
        get { return _instance; }
    }

    void Awake() {
        if (_instance != null) {
            Debug.Log("Trying to set two instances of Audio Controller");
        } else {
            _instance = this;
        }
        // collect audio sources
        AudioSource[] sources = this.GetComponents<AudioSource>();
        if(sources.Length < 2) {
            Debug.Log("Not enough audio sources added");
        } else {
            mainGameMusicAudioSrc = sources[0];
            heartRateAudioSrc = sources[1];

			heartRateAudioSrc = sources[3];
			heartRateAudioSrc = sources[4];
			bearEnterAudioSrc = sources[5];
			bearExitAudioSrc = sources[6];
			toolPickupAudioSrc = sources[7];
			toolDropAudioSrc = sources[8];
			defibulatorSurgeAudioSrc = sources[9];
			doctorDashAudioSrc = sources[10];
			doctorBumpAudioSrc = sources[11];
			surgeryEnterAudioSrc = sources[12];
			surgeryExitAudioSrc = sources[13];
			surgeryInteractAudioSrc = sources[14];
			waterBucketDumpAudioSrc = sources[15];
			waterBucketFillAudioSrc = sources[16];
			fireLoopAudioSrc = sources[17];
			anestheticMachineFillAudioSrc = sources[18];
			raccoonStealAudioSrc = sources[19];
			operationSuccessfulAudioSrc = sources[20];
			tutorialTaskCompleteAudioSrc = sources[21];
			tutorialBeginMusicSrc = sources[22];
			tutorialLoopMusicSrc = sources[23];
			dashChargeSrc = sources[24];
			playAddTutorialCircleSrc = sources[25];
        }
		//toolPickupAudioSrc.clip = toolPickup;
		//toolPickupAudioSrc.Play();

		//toolDropAudioSrc.clip = toolDrop;
		//toolDropAudioSrc.PlayDelayed(toolPickup.length);

		//defibulatorSurgeAudioSrc.clip = defibulatorSurge;
		//defibulatorSurgeAudioSrc.PlayDelayed(toolDrop.length);

		//doctorBumpAudioSrc.clip = doctorBump;
		//doctorBumpAudioSrc.PlayDelayed(defibulatorSurge.length);

		//surgeryInteractAudioSrc.clip = surgeryInteract;
		//surgeryInteractAudioSrc.PlayDelayed(doctorBump.length);

		//waterBucketDumpAudioSrc.clip = waterBucketDump;
		//waterBucketDumpAudioSrc.PlayDelayed(surgeryInteract.length);

		//waterBucketFillAudioSrc.clip = waterBucketFill;
		//waterBucketFillAudioSrc.PlayDelayed(waterBucketDump.length);

		//fireLoopAudioSrc.clip = fireLoop;
		//fireLoopAudioSrc.PlayDelayed(waterBucketFill.length);

		//anestheticMachineFillAudioSrc.clip = anestheticMachineFill;
		//anestheticMachineFillAudioSrc.PlayDelayed(fireLoop.length);

		//raccoonStealAudioSrc.clip = raccoonSteal;
		//raccoonStealAudioSrc.PlayDelayed(anestheticMachineFill.length);

		//operationSuccessfulAudioSrc.clip = operationSuccessful;
		//operationSuccessfulAudioSrc.PlayDelayed(raccoonSteal.length);

		//tutorialTaskCompleteAudioSrc.clip = tutorialTaskComplete;
		//tutorialTaskCompleteAudioSrc.PlayDelayed(operationSuccessful.length);


		//
    }
    // Use this for initialization
    void Start () {
		if (SceneManager.GetActiveScene().name == "Tutorial")
		{
			tutorialBeginMusicSrc.clip = tutorialBeginMusic;
			tutorialBeginMusicSrc.Play();
			Invoke( "PlayTutorialMusicLoop", tutorialBeginMusic.length);
		}
		else 
		{
			mainGameMusicAudioSrc.Play();
		}
	}

	// Update is called once per frame
	void Update()
	{
				
	}

    public void PlayHeartMonitorBeep() {
		// set clip and play audio
		//Debug.Log("Beep");
        heartRateAudioSrc.clip = heartMonitorBeep;
        heartRateAudioSrc.Play();
    }

    public void PlayHeartMonitorLong() {
        // set clip and play audio
        heartRateAudioSrc.clip = heartMonitorLong;
        heartRateAudioSrc.Play();
    }

	public void PlayBearEnter()
	{
		bearEnterAudioSrc.clip = bearEnter;
		bearEnterAudioSrc.Play();
	}

	public void PlayBearExit()
	{
		bearExitAudioSrc.clip = bearExit;
		bearExitAudioSrc.Play();
	}

	public void PlayToolPickup()
	{
		toolPickupAudioSrc.clip = toolPickup;
		toolPickupAudioSrc.Play();
	}

	public void PlayToolDrop()
	{
		toolDropAudioSrc.clip = toolPickup;
		toolDropAudioSrc.Play();
	}

	public void PlayDefibulatorSurge()
	{
		defibulatorSurgeAudioSrc.clip = defibulatorSurge;
		defibulatorSurgeAudioSrc.Play();
	}

	public void PlayDoctorDash()
	{
		doctorDashAudioSrc.clip = doctorDash;
		doctorDashAudioSrc.Play();
	}

	public void PlayDoctorBump()
	{
		doctorBumpAudioSrc.clip = doctorBump;
		doctorBumpAudioSrc.Play();
	}

	public void PlaySurgeryEnter()
	{
		surgeryEnterAudioSrc.clip = surgeryEnter;
		surgeryEnterAudioSrc.Play();
	}

	public void PlaySurgeryExit()
	{
		surgeryExitAudioSrc.clip = surgeryExit;
		surgeryExitAudioSrc.Play();
	}

	public void PlaySurgeryInteract()
	{
		surgeryInteractAudioSrc.clip = surgeryInteract;
		surgeryInteractAudioSrc.Play();
	}

	public void PlayWaterBucketDump()
	{
		surgeryInteractAudioSrc.clip = surgeryInteract;
		surgeryInteractAudioSrc.Play();
	}

	public void PlayWaterBucketFill()
	{
		waterBucketFillAudioSrc.clip = waterBucketFill;
		waterBucketFillAudioSrc.Play();
	}

	public void PlayFireLoop()
	{
		fireLoopAudioSrc.clip = fireLoop;
		fireLoopAudioSrc.Play();
	}

	public void StopFireLoop()
	{
		fireLoopAudioSrc.Stop();
	}

	public void PlayAnestheticMachineFill()
	{
		anestheticMachineFillAudioSrc.clip = anestheticMachineFill;
		anestheticMachineFillAudioSrc.Play();
	}

	public void PlayRaccoonSteal()
	{
		raccoonStealAudioSrc.clip = raccoonSteal;
		raccoonStealAudioSrc.Play();
	}

	public void PlayOperationSuccessful()
	{
		operationSuccessfulAudioSrc.clip = operationSuccessful;
		operationSuccessfulAudioSrc.Play();
	}

	public void PlayTutorialTaskComplete()
	{
		tutorialTaskCompleteAudioSrc.clip = tutorialTaskComplete;
		tutorialTaskCompleteAudioSrc.Play();
	}

    public void PlayTutorialCircleAdded() {
        // FIXME
    }

	public void PlayTutorialMusicLoop()
	{
		tutorialLoopMusicSrc.clip = tutorialLoopMusic;
		tutorialLoopMusicSrc.Play();
	}
	public void PlayDashCharge()
	{
		dashChargeSrc.clip = dashCharge;
		dashChargeSrc.Play();
	}
}