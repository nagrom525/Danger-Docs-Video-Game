using UnityEngine;
using System.Collections;

public class RedRingNotification : MonoBehaviour {
    public float rotationsPerSecond = 0.5f;
    public GameObject redRingPrefab;

    private GameObject redRingInstance;

    // Use this for initialization
    void Start () {
        redRingInstance = Instantiate(redRingPrefab, this.transform.position, Quaternion.identity, this.gameObject.transform) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
        float t = Time.deltaTime * rotationsPerSecond;
        float degreesToRotate = t * 360.0f;
        redRingInstance.transform.Rotate(new Vector3(0.0f, 0.0f, degreesToRotate));
	}
}
