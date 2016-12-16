using UnityEngine;
using System.Collections;

// base class for Tool, all tools derive from this class and must implement getToolType
// also abstract fuctions can be implemented if there is shared code across all types of tool
public abstract class Tool : MonoBehaviour {
    public enum ToolType {NONE, SCALPEL, GAUZE, FORCEPS, SUTURE, CANISTER, DEFIBULATOR, BUCKET}

    public Material originalMaterial;
	public GameObject highlightedComponent;
	public bool highlighted = false;
    public abstract ToolType GetToolType();
    public abstract void OnDoctorInitatedInteracting();
    public abstract void OnDoctorTerminatedInteracting();


	void Awake() {
		// If you didn't set the material, get it automajically.
		if (originalMaterial == null) {
			originalMaterial = getGameObjectMaterial(highlightedComponent);
		}
	}



	public Material getGameObjectMaterial(GameObject go) {
		Renderer rend = go.GetComponentInChildren<Renderer>();
		Material mat = null;
		
		if (rend != null) { 
			mat = rend.material;
		}

		return mat;
	}

	public virtual void disableHighlighting() {
		MeshRenderer mr = highlightedComponent.GetComponentInChildren<MeshRenderer> ();
		if (mr != null) {
			mr.material = originalMaterial;
		}
		highlighted = false;
	}

	public virtual void enableHighlighting(Material hlm) {
		MeshRenderer mr = highlightedComponent.GetComponentInChildren<MeshRenderer> ();
		if (mr != null) {
			mr.material = hlm;
		}
		highlighted = true;
	}

    public void flashMaterial(Material newMaterial, float time) { 
       MeshRenderer mr = highlightedComponent.GetComponentInChildren<MeshRenderer>();
        if (mr != null) {
            mr.material = newMaterial;
        }
        Invoke("resetMaterial", time);
    }

    private void resetMaterial() {
        MeshRenderer mr = highlightedComponent.GetComponentInChildren<MeshRenderer>();
        if (mr != null) {
            mr.material = originalMaterial;
        }
    }
}
