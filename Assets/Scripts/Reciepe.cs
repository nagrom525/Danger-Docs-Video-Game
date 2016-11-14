using UnityEngine;
using System.Collections;

public class Reciepe : MonoBehaviour {
    public enum ReciepeState { CUT_OPEN, PULL_OUT_STICK, SOAK_BLOOD, STICH_BODY}

    public static ReciepeState[] scene1ReciepeElements = new ReciepeState[4] { ReciepeState.CUT_OPEN, ReciepeState.PULL_OUT_STICK, ReciepeState.SOAK_BLOOD, ReciepeState.STICH_BODY};
}
