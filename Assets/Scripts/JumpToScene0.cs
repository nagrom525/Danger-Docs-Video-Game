using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class JumpToScene0 : MonoBehaviour {
    public float TimeToJump = 5.0f;
	// Use this for initialization
	void Start () {
        Invoke("JumpToScene0Func", TimeToJump);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void JumpToScene0Func() {
        SceneManager.LoadScene(0);
    }
}
