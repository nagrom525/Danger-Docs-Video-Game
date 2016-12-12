using UnityEngine;
using System.Collections;

public class SceneTransitionController : MonoBehaviour {

	public Transform 	cameraTransitionStartTransform;
	public GameObject	cam;
	public CanvasGroup 	blackCanvasGroup;
	public CanvasGroup 	whiteCanvasGroup;
	public bool 		inEditor;
	public float 		zoomOutDuration = 3f;
	public Transform 	cameraDefaultTransform;

	public float 		blackTransitionRate = .03f;


	void Awake()
	{
		if (!inEditor)
		{
			blackCanvasGroup.alpha = 1f;
			whiteCanvasGroup.alpha = 0f;
		}
		else
		{
			blackCanvasGroup.alpha = 0f;
			whiteCanvasGroup.alpha = 0f;
		}
	}

	// Use this for initialization
	void Start () {

		if (!inEditor)
		{
			//disable weighted camera during the transition
			cam.transform.parent.gameObject.GetComponent<WeightedCamera>().enabled = false;
			cam.transform.position = cameraTransitionStartTransform.position;
			cam.transform.rotation = cameraTransitionStartTransform.rotation;
			//enable it when transition is complete
			StartCoroutine(SceneStartTransition());
		}
		else
		{
			//don't do anything to the camera
		}
	}

	IEnumerator SceneStartTransition()
	{
		//fade out canvas group

		yield return new WaitForSeconds(1f);
		while (blackCanvasGroup.alpha > 0f)
		{
			blackCanvasGroup.alpha -= blackTransitionRate;
			yield return new WaitForEndOfFrame();
		}
		blackCanvasGroup.alpha = 0f;

		float t = 0f;
		//zoom camera out
		while (t < zoomOutDuration)
		{
			Vector3 newPos = cam.transform.position - .3f*(cam.transform.forward);
			cam.transform.position = newPos;
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;
			if (t > zoomOutDuration/2)
				whiteCanvasGroup.alpha += blackTransitionRate;
		}

		while (whiteCanvasGroup.alpha < 1f)
		{
			whiteCanvasGroup.alpha += blackTransitionRate;
			yield return new WaitForEndOfFrame();
		}
		//fade to white
		//setup up camera
		//enable weighted
		cam.transform.parent.gameObject.GetComponent<WeightedCamera>().enabled = true;
		ResetCameraTransform();

		while (whiteCanvasGroup.alpha > 0f)
		{
			whiteCanvasGroup.alpha -= blackTransitionRate*2f;
			yield return new WaitForEndOfFrame();
		}
		yield return null;
	}

	void ResetCameraTransform()
	{
		cam.transform.position = cameraDefaultTransform.position;
		cam.transform.rotation = cameraDefaultTransform.rotation;
        TutorialEventController.Instance.InformTutorialStart();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
