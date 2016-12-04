using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This is outside of the Utils Class
public enum BoundsTest {
	center,
	onScreen,
	offScreen
}



public class Utils : MonoBehaviour {
	public static Bounds BoundsUnion ( Bounds b0, Bounds b1 ) {
		// If the size of one of the bounds is Vector3.zero, ignore that one.
		if ( b0.size == Vector3.zero && b1.size != Vector3.zero ) {
			return b1;
		} else if ( b0.size != Vector3.zero && b1.size == Vector3.zero ) {
			return b0;
		} else if ( b0.size == Vector3.zero && b1.size == Vector3.zero) {
			return b0;
		}
		// Stretch b0 to include the b1.min and b1.max
		b0.Encapsulate(b1.min);
		b0.Encapsulate(b1.max);
		return b0;
	}

	public static Bounds CombineBoundsOfChildren (GameObject go) {
		// Create a empty bounds b
		Bounds b = new Bounds(Vector3.zero, Vector3.zero);
		// If this GameObject has a Renderer Comp...
		if ( go.GetComponent<Renderer>() != null ) {
			// Expand b to contain the Collider's Bounds
			b = BoundsUnion(b, go.GetComponent<Renderer>().bounds);
		}
		// If thuis GO has a collider ciomp....
		if (go.GetComponent<Collider>() != null) {
			if (go.GetComponent<Renderer>() != null) {
				b = BoundsUnion(b, go.GetComponent<Renderer>().bounds);
			}
		}
		// Iterate through each child of this gameObject.transform
		foreach(Transform t in go.transform) {
			// Expand b to contain their Bounds as well
			b = BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));
		}

		return b;
	}


	// Make a static read-only public property camBounds
	static public Bounds camBounds {
		get {
			// If _camBounds hasn't been set yet/
			if (_camBounds.size == Vector3.zero) {
				SetCameraBounds ();
			}

			return _camBounds;
		}
	}
	static private Bounds _camBounds;

	public static void SetCameraBounds(Camera cam = null) {
		// If no camera was passed in, use the main camera
		if (cam == null) cam = Camera.main;
		// Tjis makes a couple important assumptions about the camera!
		//	 1. The camera is orthoigraphic
		//   2. The camera is at a rotation of R:[0, 0, 0]

		// Make Vector3s at the topLeft and bottiom right of the Screen coords
		Vector3 topLeft = new Vector3(0, 0, 0);
		Vector3 bottomRight = new Vector3(Screen.width, Screen.height, 0);

		// Convert these to world coordinates
		Vector3 boundTLN = cam.ScreenToWorldPoint(topLeft);
		Vector3 boundBRF = cam.ScreenToWorldPoint(bottomRight);

		// Adjust the z to be at the near and far Camera clipping planes.
		boundTLN.z += cam.nearClipPlane;
		boundBRF.z += cam.farClipPlane;

		// Find the center of the bounds
		Vector3 center = (boundTLN + boundBRF) / 2f;
		_camBounds = new Bounds (center, Vector3.zero);
		// Expand _camBounds to encap the extents
		_camBounds.Encapsulate(boundTLN);
		_camBounds.Encapsulate(boundBRF);
	}


	public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.center) {
		return BoundsInBoundsCheck (camBounds, bnd, test);
	}

	public static Vector3 BoundsInBoundsCheck(Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.onScreen) {
		// Get the center of lilB
		Vector3 pos = lilB.center;

		//Initialize the offset at [0, 0, 0]
		Vector3 off = Vector3.zero;

		switch (test) {
		// The center test determines what offset would have to be applied to lilB to move its cetenr back instide bigB
		case BoundsTest.center:
			// If the center is contained retuirn vec3 zero
			if (bigB.Contains (pos)) {
				return Vector3.zero;
			}
			// If not contained, find the offset
			if (pos.x > bigB.max.x) {
				off.x = pos.x - bigB.max.x;
			} else if (pos.x < bigB.max.x) {
				off.x = pos.x - bigB.min.x;
			}

			if (pos.y > bigB.max.y) {
				off.y = pos.y - bigB.max.y;
			} else if (pos.y < bigB.min.y) {
				off.y = pos.y - bigB.min.y;
			}

			if (pos.z > bigB.max.z) {
				off.z = pos.z - bigB.max.z;
			} else if (pos.z < bigB.min.z) {
				off.z = pos.z - bigB.min.z;
			}
			return off;

		// The onScreen Test determines what ogff would have to be applied to keep all of lilB inside bigB.
		case BoundsTest.onScreen:
			// Find whether bigB conatains all of lilB
			if (bigB.Contains(lilB.min) && bigB.Contains(lilB.max)) {
				return Vector3.zero;
			}
			// if not, find the offset
			if (lilB.max.x > bigB.max.x) {
				off.x = lilB.max.x - bigB.max.x;
			} else if (lilB.min.x < bigB.min.x) {
				off.x = lilB.min.x - bigB.min.x;
			}

			if (lilB.max.y > bigB.max.y) {
				off.y = lilB.max.y - bigB.max.y;
			} else if (lilB.min.y < bigB.min.y) {
				off.y = lilB.min.y - bigB.min.y;
			}

			if (lilB.max.z > bigB.max.z) {
				off.z = lilB.max.z - bigB.max.z;
			} else if (lilB.min.z < bigB.min.z) {
				off.z = lilB.min.z - bigB.min.z;
			}
			return off;

		// The offscreen test determines what offset would need to be apoplied to move any tny part opf lilB inside BigB
		case BoundsTest.offScreen:
			bool cMin = bigB.Contains(lilB.min);
			bool cMax = bigB.Contains(lilB.max);
			if ( cMin || cMax) {
				return Vector3.zero;
			}


			// if not, find the offset
			if (lilB.min.x > bigB.max.x) {
				off.x = lilB.min.x - bigB.max.x;
			} else if (lilB.max.x < bigB.min.x) {
				off.x = lilB.max.x - bigB.min.x;
			}

			if (lilB.min.y > bigB.max.y) {
				off.y = lilB.min.y - bigB.max.y;
			} else if (lilB.max.y < bigB.min.y) {
				off.y = lilB.max.y - bigB.min.y;
			}

			if (lilB.min.z > bigB.max.z) {
				off.z = lilB.min.z - bigB.max.z;
			} else if (lilB.max.z < bigB.min.z) {
				off.z = lilB.max.z - bigB.min.z;
			}
			return off;	
		}

		return Vector3.zero;
	}

	static public Material[] GetAllMaterials (GameObject go) {
		List<Material> mats = new List<Material> ();
		if (go.GetComponent<Renderer>() != null) {
			mats.Add (go.GetComponent<Renderer> ().material);
		}
		foreach ( Transform t in go.transform ) {
			mats.AddRange (GetAllMaterials(t.gameObject));
		}
		return mats.ToArray ();
	}




	public static GameObject FindTaggedParent(GameObject go) {
		// if this gameobject has a tag
		if (go.tag != "Untagged") {
			// then return this gameObject
			return go;
		}
		// if there is no parent of this transform
		if (go.transform.parent == null) {
			// We've reached the top with no interesting tag.
			return null;
		}
		// Otherwise, recurse for great tagging!
		return FindTaggedParent(go.transform.parent.gameObject);
	}
	// Handles the event where a transform is passed in. Get the game object and proceed with recurshhhhun
	public static GameObject FindTaggedParent(Transform t) {
		return FindTaggedParent (t.gameObject);
	}





}
