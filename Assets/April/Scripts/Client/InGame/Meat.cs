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

    // public MeatState currentState = MeatState.Raw;

    public MeatState State 
    {
        get 
        {
            if (progressValue <= 0)
            {
                return MeatState.Raw;
            }
            else if (progressValue <= 40f)
            {
                return MeatState.Medium;
            }
            else if (progressValue <= 90f)
            {
                return MeatState.WellDone;
            }
            else
            {
                return MeatState.Burned;
            }
        }
    }

    public float progressValue;
}
