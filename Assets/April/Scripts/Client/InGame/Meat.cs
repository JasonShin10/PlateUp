using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Meat : MonoBehaviour
{
    public Slider slider;
    public float progressValue;

    public enum MeatState
    {
        Raw,
        Medium,
        WellDone,
        Burned
    }

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

    public void Update()
    {
        if (slider != null)
        {
            slider.value = progressValue;
        }
    }
}
