using UnityEngine;
using System.Collections;

public class Flame : MonoBehaviour {
    private enum Direction { NORTH, SOUTH, EAST, WEST};

    public float fireGridScale = 1.0f;
    public float timeFireSpawnDelay = 2.0f;

    // flame graph connections
    public delegate void ChildEvent();
    public ChildEvent onChildDestroyed;
    private Flame parent;

    private float lastFireTime;
    private Direction nextFireSpawnDirection;
    


    // Use this for initialization
    void Start () {
        lastFireTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setParent(Flame parent) {
        this.parent = parent;
        parent.onChildDestroyed += OnDestroy;
    }

    void OnDestroy() {

    }
}
