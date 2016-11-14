using UnityEngine;
using System.Collections;

public class AnestheticMachine : Interactable {

	// The Anesthetic Machine does not require a tool to interact ... yet!
	protected override Tool.ToolType RequiredToolType() {
		return Tool.ToolType.NONE;
	}

	// TODO: ALL the code related to this thing actually doing something
	// to the patient.
}
