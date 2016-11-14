using UnityEngine;
using System.Collections;

// base class for Tool, all tools derive from this class and must implement getToolType
// also abstract fuctions can be implemented if there is shared code across all types of tool
public abstract class Tool : MonoBehaviour {
    public enum ToolType {NONE, TYPE_1, TYPE_2, TYPE_3, TYPE_4, SCALPEL, GAUZE, FORCEPS, SUTURE, CANISTER}

    public abstract ToolType GetToolType();

    public abstract void OnDoctorInitatedInteracting();
    public abstract void OnDoctorTerminatedInteracting();
}
