using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Patient))]
public class PatientEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Patient myScript = (Patient)target;
		if (GUILayout.Button("Scalpel Surgery"))
		{
			myScript.OnCutPatientOpen(0);
		}
		if (GUILayout.Button("Suture Surgery"))
		{
			myScript.OnSuture(0);
		}
		if (GUILayout.Button("Gauze Surgery"))
		{
			myScript.OnSoakBlood(0);
		}
	}
}
