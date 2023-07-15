using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : MonoBehaviour
{
    public enum MeatState
    {
        Raw,
        Medium,
        WellDone,
        Burned
    }
    public MeatState currentState = MeatState.Raw;
}
