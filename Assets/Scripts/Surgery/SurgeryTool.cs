using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for surgery tools.
/// </summary>
public class SurgeryTool : MonoBehaviour {

	public bool activated;

	public virtual void Activate()
	{
		activated = true;
	}

	public virtual void Deactivate()
	{
		activated = false;
	}

	public virtual void SuccesfulUse()
	{
		//send message
	}

	public virtual void UnsuccesfulUse()
	{
		//send message
	}

	public void OnJoystickMovement(Vector3 direction)
	{
		pos += direction * .2f * Time.deltaTime;
	}

	private Vector3 pos
	{
		get
		{
			return this.transform.position;
		}
		set
		{
			this.transform.position = value;
		}
	}

}
