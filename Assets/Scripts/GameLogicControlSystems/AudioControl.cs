using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {


    public AudioClip heartMonitorBeep;
    public AudioClip heartMonitorLong;


    private AudioSource mainGameMusicAudioSrc;
    private AudioSource heartRateAudioSrc;

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
        }
    }
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayHeartMonitorBeep() {
		// set clip and play audio
		Debug.Log("Beep");
        heartRateAudioSrc.clip = heartMonitorBeep;
        heartRateAudioSrc.Play();
    }

    public void PlayHeartMonitorLong() {
        // set clip and play audio
        heartRateAudioSrc.clip = heartMonitorLong;
        heartRateAudioSrc.Play();
    }
}
