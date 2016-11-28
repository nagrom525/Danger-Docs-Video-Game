using UnityEngine;
using System.Collections;

public class ToolDrop : MonoBehaviour {

	public Tool.ToolType type;
	public bool toolSpawned;
	public GameObject[] toolPrefabs;
	public GameObject[] models;

	private float t;

	// Use this for initialization
	void Start () {
		toolSpawned = false;
		Invoke("ToggleModel", .5f);
	}

	void ToggleModel() {
		switch (type)
		{
			case Tool.ToolType.SUTURE:
				models[0].SetActive(true);
				break;
			case Tool.ToolType.SCALPEL:
				models[1].SetActive(true);
				break;
			case Tool.ToolType.GAUZE:
				models[2].SetActive(true);
				break;
			case Tool.ToolType.CANISTER:
				models[3].SetActive(true);
				break;
			case Tool.ToolType.BUCKET:
				models[4].SetActive(true);
				break;
			case Tool.ToolType.DEFIBULATOR:
				models[5].SetActive(true);
				break;
		}	
	}

	void LateUpdate() {
		t += Time.deltaTime;
		transform.eulerAngles = new Vector3(0f, 0f, 10f*Mathf.Sin(3*t));
		Vector3 newPos = transform.position + new Vector3(0f, -.15f, 0f);
		transform.position = newPos;
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.y < 2f && !toolSpawned)
		{
			switch (type)
			{
				case Tool.ToolType.SUTURE:
					var go = Instantiate(toolPrefabs[0]);
					go.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
					break;
				case Tool.ToolType.SCALPEL:
					go = Instantiate(toolPrefabs[1]);
					go.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
					break;
				case Tool.ToolType.GAUZE:
					go = Instantiate(toolPrefabs[2]);
					go.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
					break;
				case Tool.ToolType.CANISTER:
					go = Instantiate(toolPrefabs[3]);
					go.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
					break;
				case Tool.ToolType.BUCKET:
					go = Instantiate(toolPrefabs[4]);
					go.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
					break;
				case Tool.ToolType.DEFIBULATOR:
					go = Instantiate(toolPrefabs[5]);
					go.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
					break;
			}
			Invoke("Kill", 2f);
			toolSpawned = true;
		}
	}

	public void Kill() {
		Destroy(this.gameObject);
	}
}
