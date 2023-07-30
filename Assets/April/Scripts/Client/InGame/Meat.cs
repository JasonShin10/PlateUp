using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;
public class Meat : Food
{
    public Slider slider;
    

    private void Start()
    {
        slider = GetComponentInChildren<Slider>(true);
        slider.maxValue = 90f;
    }
    public override int CookingState => (int)State;

    //public override string CookingState
    //{
    //    get
    //    {
    //        return CurrenState.ToString();
    //    }
    //}

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

    public override void ShowUI()
    {
        slider.gameObject.SetActive(true);
    }

    public override void HideUI()
    {
        slider.gameObject.SetActive(false);
    }
    void Update()
    {
        if (slider != null)
        {
            slider.value = progressValue;
        }
    }
}
