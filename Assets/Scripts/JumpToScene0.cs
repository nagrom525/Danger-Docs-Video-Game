using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class JumpToScene0 : MonoBehaviour {
    public float TimeToJump = 5.0f;
    private float startTime;
    // Use this for initialization
    void Start() {
        startTime = Time.unscaledTime;
    }

    // Update is called once per frame
    void Update() {
        if ((Time.unscaledTime - startTime) >= TimeToJump) {
            SceneManager.LoadScene(0);
        }
    }
}
