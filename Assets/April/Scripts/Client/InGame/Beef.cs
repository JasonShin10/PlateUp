using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;

namespace April
{
    public class Beef : Food
    {
        public override MenuList MenuType => MenuList.Beef;

      

        public enum BeefState
        {
            Raw,
            Medium,
            WellDone,
            Burned
        }

        public Slider slider;
        private Renderer BeefRenderer;
        Color BeefColor;

        private void Start()
        {
            slider = GetComponentInChildren<Slider>(true);
            slider.maxValue = 90f;
            BeefRenderer = GetComponentInChildren<Renderer>(true);
        }
        public override int CookingState => (int)State;

        //public override string CookingState
        //{
        //    get
        //    {
        //        return State.ToString();
        //    }
        //}
        
        public BeefState State
        {
            get
            {
                if (progressValue <= 0)
                {
                    return BeefState.Raw;
                }
                else if (progressValue <= 40f)
                {
                    return BeefState.Medium;
                }
                else if (progressValue <= 90f)
                {
                    return BeefState.WellDone;
                }
                else
                {
                    return BeefState.Burned;
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
                case BeefState.Raw:
                    BeefColor = Color.red;
                    break;
                case BeefState.Medium:
                    BeefColor = new Color(0.8f, 0.3f, 0.1f);
                    break;
                case BeefState.WellDone:
                    BeefColor = new Color(0.5f, 0.2f, 0.1f);
                    break;
                case BeefState.Burned:
                    BeefColor = Color.black;
                    break;
            }
            BeefRenderer.material.color = BeefColor;

            if (slider != null)
            {
                slider.value = progressValue;
            }
        }
    }

}

