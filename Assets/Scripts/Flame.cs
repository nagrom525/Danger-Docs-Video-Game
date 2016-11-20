using UnityEngine;
using System.Collections;

public class Flame : MonoBehaviour {
    private enum Direction { NORTH, EAST, SOUTH, WEST};
    private enum CanSpawnState { OPEN, PERMINATLY_CLOSED, CHECK};

    public float fireGridScale = 1.0f;
    public float timeFireSpawnDelay = 2.0f;
    public GameObject flamePrefab;

    // flame graph connections
    private delegate void ChildEvent(Direction d);
    private ChildEvent onDestroyEvent;
    private CanSpawnState[] canSpawn = new CanSpawnState[4] { CanSpawnState.CHECK, CanSpawnState.CHECK, CanSpawnState.CHECK, CanSpawnState.CHECK}; // represents if we can spawn fire in any of the 4 directions
    //private bool hasParent = false;

    private float lastFireTime;
    private Direction nextFireSpawnDirection;
  
    


    void Awake() {
        nextFireSpawnDirection = getRandomDirection();
    }

    // Use this for initialization
    void Start () {
        lastFireTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	    if(TimeToSpawnFire()) {
            if (CanSpawnInRegion(nextFireSpawnDirection)) {
                SpawnFire(nextFireSpawnDirection);
            }
        }
        // reset time intervial and advance direction
        lastFireTime = Time.time;
        nextFireSpawnDirection = GetNextDirection(nextFireSpawnDirection);
	}

    // called when a spawned child is destroyed
    private void OnChildDestroyed(Direction d) {

    }

    // unity engine event.. called when the object is destroyed
    void OnDestroy() {
        
    }

    //// direction spawned is the direction the child is in relative to the parent
    //private void SetParent(Direction directionSpawned) {
    //    //hasParent = true;
    //    dire
    //}

    // returns true if the space is open to spawning fire
    // no water or other object already there
    // no other fire already there.
    private bool CanSpawnInRegion(Direction d) {
        // need to fill in this code with tag checking
        return true;
    }

    private void SpawnFire(Direction d) {
        Instantiate(flamePrefab, GetSpawnLocation(d), Quaternion.identity);
    }

    private Direction getRandomDirection() {
        return (Direction)Random.Range(0, 3);
    }

    private bool TimeToSpawnFire() {
        return (Time.time - lastFireTime) > timeFireSpawnDelay;
    }

    // gets the new location based on the fireGridScale
    private Vector3 GetSpawnLocation(Direction d) {
        Vector3 currentLocation = this.transform.position;
        switch (d) {
            case Direction.NORTH:
                return new Vector3(currentLocation.x, currentLocation.y, currentLocation.z + fireGridScale);
            case Direction.EAST:
                return new Vector3(currentLocation.x + fireGridScale, currentLocation.y, currentLocation.z);
            case Direction.SOUTH:
                return new Vector3(currentLocation.x, currentLocation.y, currentLocation.z - fireGridScale);
            case Direction.WEST:
                return new Vector3(currentLocation.x - fireGridScale, currentLocation.y, currentLocation.z);
            default:
                return currentLocation;
        }
    }

    // gets next direction in a clockwise manner (given current dir)
    private Direction GetNextDirection(Direction dir) {
        switch (dir) {
            case Direction.NORTH:
                return Direction.EAST;
            case Direction.EAST:
                return Direction.SOUTH;
            case Direction.SOUTH:
                return Direction.WEST;
            case Direction.WEST:
                return Direction.NORTH;
        }
        return Direction.NORTH;
    }

    // gets the opposite direction
    // for example: dir=east returns west. and dir=north returns south
    private Direction getOpositeDirection(Direction dir) {
        switch (dir) {
            case Direction.NORTH:
                return Direction.SOUTH;
            case Direction.SOUTH:
                return Direction.NORTH;
            case Direction.EAST:
                return Direction.WEST;
            case Direction.WEST:
                return Direction.EAST;
        }
        return Direction.NORTH;
    }
}
