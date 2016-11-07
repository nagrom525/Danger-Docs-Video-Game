using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// Handles game flow. Start game, end game, pause game, reset game.
/// </summary>
public class GameController : MonoBehaviour {

    private static GameController _instance;
    public static GameController Instance
    {
        get { return _instance; }
    }

    public DoctorEvents docEvents;

    public float        startTime;
    public float        timeRemaining;

    public Text         timerText;

    public bool         gameActive;

	// Use this for initialization
	void Start () {
        _instance = this;

        docEvents = GameObject.Find("EventSystem").GetComponent<DoctorEvents>();
        if (docEvents == null)
            Debug.Log("Couldn't find Doctor Events component attached to EventSystem!");
	}

    /// <summary>
    /// Stops timers. Show pause menu
    /// </summary>
    public void PauseGame()
    {
        gameActive = false;
    }

    public void StartGame()
    {
        gameActive = true;
    }

    public void EndGame()
    {
        gameActive = false;
    }

    public void ResetGame()
    {
        gameActive = false;
        //Reset timer
        timeRemaining = startTime;
    }

	// Update is called once per frame
	void Update () {
	    if (gameActive)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = FormatTime(timeRemaining);
            if (timeRemaining < 60f)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.white;
            }

            if (timeRemaining <= 0f)
            {
                EndGame();
            }
        }
	}

    string FormatTime(float value)
    {
        TimeSpan t = TimeSpan.FromSeconds(value);
        return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }
}
