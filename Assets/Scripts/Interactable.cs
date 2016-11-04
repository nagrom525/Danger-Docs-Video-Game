using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour {


    // internal method for erturning the type that this interactable object requires
    protected abstract Tool.ToolType RequiredToolType();

    // called by the doctor to initate interaction with an interactive
    // (1) could have callback called in interactingDoctor when event is called if this is an important detail
    // (2) have to check tool type to make sure it is compatable and also could call a function in heldTool to let it know the interaction that is happening
    protected abstract void DocterInteracting(Doctor interactingDoctor, Tool heldTool);
}
