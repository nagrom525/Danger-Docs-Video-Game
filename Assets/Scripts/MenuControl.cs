using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RestartLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
}
