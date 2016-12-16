using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SkipTutorial : MonoBehaviour {
	string scenename = "";
	// Use this for initialization
	void Start () {
		scenename = SceneManager.GetActiveScene().name;
	}
	
	// Update is called once per frame
	void Update () {
		if (scenename == "Tutorial")
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SceneManager.LoadScene("Debug_Ben_2");
			}
		}
		else {
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SceneManager.LoadScene("Tutorial");
			}
				
		}

	}
}
