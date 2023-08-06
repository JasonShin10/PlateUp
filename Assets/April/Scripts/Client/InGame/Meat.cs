using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;

namespace April
{
    public class Meat : Food
    {
        public enum MeatState
        {
            Raw,
            Medium,
            WellDone,
            Burned
        }

        public Slider slider;
        private Renderer meatRenderer;
        Color meatColor;

        private void Start()
        {
            slider = GetComponentInChildren<Slider>(true);
            slider.maxValue = 90f;
            meatRenderer = GetComponentInChildren<Renderer>(true);
        }
        public override int CookingState => (int)State;

        //public override string CookingState
        //{
        //    get
        //    {
        //        return CurrenState.ToString();
        //    }
        //}

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
            switch (State)
            {
                case MeatState.Raw:
                    meatColor = Color.red;
                    break;
                case MeatState.Medium:
                    meatColor = new Color(0.8f, 0.3f, 0.1f);
                    break;
                case MeatState.WellDone:
                    meatColor = new Color(0.5f, 0.2f, 0.1f);
                    break;
                case MeatState.Burned:
                    meatColor = Color.black;
                    break;
            }
            meatRenderer.material.color = meatColor;

            if (slider != null)
            {
                slider.value = progressValue;
            }
        }
    }

}

