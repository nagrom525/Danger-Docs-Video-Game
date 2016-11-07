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

    public static void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void ExitGame() {
        Application.Quit();
    }

    public static void MainMenu() {
        SceneManager.LoadScene(0);
    }
}
