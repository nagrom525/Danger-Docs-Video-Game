using UnityEngine;
using System.Collections;
using InControl;

public class TutorialController : MonoBehaviour {

	public Tutorial tutorial;

	// Update is called once per frame
	void Update () {
		// If there's at least one player, make the first player the controller
		InputDevice inputDevice = (InputManager.Devices[0] != null) ? InputManager.Devices[0] : null;

		if (inputDevice == null) {
			// No controller? Do something? Maybe?
		} else {
			checkForInput (inputDevice);
		}
	}


	public void checkForInput(InputDevice id) {
		if (id.Action1.WasPressed) {
			// If the first player pressed A, then go to the next slide.
			advanceSlides();
		}
	}

	public void advanceSlides() {
		tutorial.incrementSlides ();
	}
}
