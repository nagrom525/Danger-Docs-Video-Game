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

	public void Activate(int playerNumber)
	{
        if (!activated)
        {
            sutureController.Sitched();
			sutureController.lastPlayerToSuture = playerNumber;
            //change mesh to closed wound
            MF.mesh = closedWound;
            activated = true; 
        }

    }
}
