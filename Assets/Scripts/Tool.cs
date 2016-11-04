using UnityEngine;
using System.Collections;

// base class for Tool, all tools derive from this class and must implement getToolType
public abstract class Tool : MonoBehaviour {
    public enum ToolType {NONE, TYPE_1, TYPE_2, TYPE_3, TYPE_4}

    public abstract ToolType GetToolType();
}
