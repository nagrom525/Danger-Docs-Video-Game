using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {
	public CanvasGroup[] slides;
	public CanvasGroup currentSlide;
	public int slideIndex;

	// Use this for initialization
	void Start () {
		slides = GetComponentsInChildren<CanvasGroup>();
		slideIndex = 0;
		currentSlide = slides[slideIndex];
	}

	void Update() {
		for (int i = 0; i < slides.Length; i++) {
			if (i != slideIndex) {
				slides [i].gameObject.SetActive (false);
			} else {
				slides [i].gameObject.SetActive (true);
			}
		}
		if (Input.GetButtonDown("Fire1"))
		{
			incrementSlides();
		}
	}
	
	public void incrementSlides() {
		slideIndex++;

		if (slideIndex >= slides.Length) {
			// Assumes tutorial scene is == 0, and that the main scene is == 1
			SceneManager.LoadScene("Debug_Ben_2");
			return;
		}

		currentSlide = slides [slideIndex];
	}
}
