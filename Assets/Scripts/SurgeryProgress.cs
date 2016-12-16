using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SurgeryProgress : MonoBehaviour {
    public Image progressIndicator;
    public float fillPrecentPerSecond = 20.0f;

    private bool filling = false;
    private float currentPrecentComplete;
    private float newPrecentComplete;
    private float fillStartTime;
    private float totalFillTIme;

    // Use this for initialization
    void Start() {
        DoctorEvents.Instance.onSurgeryOperationComplete += OnSurgeryComplete;
        currentPrecentComplete = DoctorEvents.Instance.GetSurgeryPrecentComplete();
        progressIndicator.fillAmount = currentPrecentComplete;
    }
	
	// Update is called once per frame
	void Update () {
        if (filling) {
            float t = (Time.time - fillStartTime) / totalFillTIme;
            
            if(t >= 1.0) {
                filling = false;
                progressIndicator.fillAmount = newPrecentComplete;
                currentPrecentComplete = newPrecentComplete;
            }
            progressIndicator.fillAmount = Mathfx.Hermite(currentPrecentComplete, newPrecentComplete, t);
        }
	}

    private void OnSurgeryComplete(float duration) {
        newPrecentComplete = DoctorEvents.Instance.GetSurgeryPrecentComplete();
        fillStartTime = Time.time;
        totalFillTIme = ((newPrecentComplete - currentPrecentComplete) * 100) / fillPrecentPerSecond;
        filling = true;
    }

    void OnDestroy() {
        DoctorEvents.Instance.onSurgeryOperationComplete -= OnSurgeryComplete;
    }
}
