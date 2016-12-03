using UnityEngine;
using System.Collections;

public class SutureHotspot : MonoBehaviour {
    public Mesh closedWound;
    private MeshFilter MF;
	public SutureController sutureController;
    public bool activated;
    void Awake()
    {
        MF = GetComponent<MeshFilter>();
    }

	public void Activate()
	{
        if (!activated)
        {
            sutureController.Sitched();
            //change mesh to closed wound
            MF.mesh = closedWound;
            activated = true; 
        }

    }
}
