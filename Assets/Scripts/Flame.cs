using UnityEngine;
using System.Collections;

public class Flame : MonoBehaviour
{
	private enum Direction { NORTH, EAST, SOUTH, WEST };
	private enum CanSpawnState { OPEN, CLOSED, CHECK };
    public static bool firePutOutOnce = false;
	private static int flameCount = 0;
    public bool motherFlame = false;
	public float fireGridScale = 1.0f;
	public float timeFireSpawnDelay = 2.0f;
	public GameObject flamePrefab;
    public GameObject actionButtonCanvas;
	private static GameObject flameAnchor = null;

	// flame graph connections
	private delegate void ChildEvent(Direction d);
	private ChildEvent onDestroyEvent;
	private CanSpawnState[] canSpawn = new CanSpawnState[4] { CanSpawnState.CHECK, CanSpawnState.CHECK, CanSpawnState.CHECK, CanSpawnState.CHECK }; // represents if we can spawn fire in any of the 4 directions
																																					//private bool hasParent = false;
	private Direction directionSpawned;

	private float lastFireTime;
	private Direction nextFireSpawnDirection;




	void Awake()
	{
		nextFireSpawnDirection = getRandomDirection();
        motherFlame = false;
        actionButtonCanvas.SetActive(false);
	}

	// Use this for initialization
	void Start()
	{
		lastFireTime = Time.time + Random.value;
		if (flameAnchor == null)
		{
			// then we need to create a new flame anchor
			flameAnchor = new GameObject("Flame Anchor");
		}
		flameCount++;
        DoctorEvents.Instance.onBucketFilled += OnBucketFilled;
        DoctorEvents.Instance.onBucketDropped += OnBucketDropped;
        DoctorEvents.Instance.onBucketPickedUp += OnBucketPickedUp;
        DoctorEvents.Instance.onBucketEmptied += OnBucketEmptied;
	}

	// Update is called once per frame
	void Update()
	{
		if (TimeToSpawnFire())
		{
			if (CanSpawnInRegion(nextFireSpawnDirection))
			{
				SpawnFire(nextFireSpawnDirection);
			}
			// reset time intervial and advance direction
			lastFireTime = Time.time + Random.value;
			nextFireSpawnDirection = GetNextDirection(nextFireSpawnDirection);
		}
	}

	// called when a spawned child is destroyed

	private void OnChildDestroyed(Direction d)
	{
		canSpawn[(int)d] = CanSpawnState.CHECK;
	}

	

	//// direction spawned is the direction the child is in relative to the parent
	//private void SetParent(Direction directionSpawned) {
	//    //hasParent = true;
	//    dire
	//}
	private void SetDirectionSpawned(Direction directionSpawned)
	{
		this.directionSpawned = directionSpawned;
	}

	// returns true if the space is open to spawning fire
	// no water or other object already there
	// no other fire already there.
	private bool CanSpawnInRegion(Direction d)
	{
		switch (canSpawn[(int)d])
		{
			case CanSpawnState.OPEN:
				return true;
			case CanSpawnState.CLOSED:
				return false;
			case CanSpawnState.CHECK:
				Collider[] colliders = Physics.OverlapSphere(GetSpawnLocation(d), 0.4f * fireGridScale);
				foreach (var collider in colliders)
				{
					if (collider.gameObject.tag == "NotFlamable")
					{
						return false;
					}
				}
				return true;
		}
		return true;
	}

	private void SpawnFire(Direction d)
	{
		Flame childFlame = (Instantiate(flamePrefab, GetSpawnLocation(d), Quaternion.identity, flameAnchor.transform) as GameObject).GetComponent<Flame>();
		childFlame.gameObject.name = this.gameObject.name + DirectionToLetter(d);
		canSpawn[(int)d] = CanSpawnState.CLOSED;
		childFlame.SetDirectionSpawned(d);
	}

	private char DirectionToLetter(Direction d)
	{
		switch (d)
		{
			case Direction.NORTH:
				return 'N';
			case Direction.EAST:
				return 'E';
			case Direction.SOUTH:
				return 'S';
			case Direction.WEST:
				return 'W';
			default:
				return '~';
		}
	}

	private Direction getRandomDirection()
	{
		return (Direction)Random.Range(0, 3);
	}

	private bool TimeToSpawnFire()
	{
		return (Time.time - lastFireTime) > timeFireSpawnDelay;
	}

	// gets the new location based on the fireGridScale
	private Vector3 GetSpawnLocation(Direction d)
	{
		Vector3 currentLocation = this.transform.position;
		switch (d)
		{
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
	private Direction GetNextDirection(Direction dir)
	{
		switch (dir)
		{
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
	private Direction getOpositeDirection(Direction dir)
	{
		switch (dir)
		{
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

	// For igniting doctors.
	void OnCollisionEnter(Collision col) {
		GameObject go = col.gameObject;
		if (go.CompareTag("Doctor")) {
			Doctor thisDoc = go.GetComponent<Doctor>();
			if (thisDoc.onFireFrames <= 0) {
				thisDoc.ignite();
			}
		}
	}

    void OnBucketFilled(float duration) {
        if (!firePutOutOnce && motherFlame) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        }
    }

    void OnBucketPickedUp(bool full) {
        if (full && !firePutOutOnce && motherFlame) {
            actionButtonCanvas.SetActive(true);
            actionButtonCanvas.GetComponent<BounceUpAndDown>().initiateBounce();
        }
    }

    void OnBucketDropped(bool full) {
        actionButtonCanvas.SetActive(false);
    }

    void OnBucketEmptied(float duration) {
        actionButtonCanvas.SetActive(false);
    }

    // unity engine event.. called when the object is destroyed
    void OnDestroy() {
        firePutOutOnce = true;
        if (onDestroyEvent != null) {
            onDestroyEvent(directionSpawned);
        }
        flameCount--;
        if (flameCount == 0) {
            DoctorEvents.Instance.InformFirePutOut();
        }
        DoctorEvents.Instance.onBucketDropped -= OnBucketDropped;
        DoctorEvents.Instance.onBucketFilled -= OnBucketFilled;
        DoctorEvents.Instance.onBucketDropped -= OnBucketDropped;
        DoctorEvents.Instance.onBucketPickedUp -= OnBucketPickedUp;
        DoctorEvents.Instance.onBucketEmptied -= OnBucketEmptied;
    }

}
