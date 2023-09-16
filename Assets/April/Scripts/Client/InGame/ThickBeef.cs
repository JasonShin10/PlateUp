using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static April.Beef;

namespace April
{
    public class ThickBeef : Food
    {
        public override MenuList MenuType => MenuList.ThickBeef;
        private Renderer thickBeefRenderer;
        public List<Material> stateMesh = new List<Material>();
        public enum ThickBeefState
        {
            Raw,
            Medium,
            WellDone,
            Burned
        }
        public Slider slider;
        [SerializeField] private BeefState state;
        public ThickBeefState CurrentThickBeefState { get; set; } = ThickBeefState.Raw;

        public BeefState State
        {
            get { return state; }
            private set { state = value; }

        }

        public override int CookingState => (int)State;

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
            if (progressValue <= 0)
            {
                if (State == BeefState.Raw)
                {
                    return;
                }
                State = BeefState.Raw;
            }
            else if (progressValue <= 40f)
            {
                if (State == BeefState.Medium)
                {
                    return;
                }
                State = BeefState.Medium;
                thickBeefRenderer.sharedMaterial = stateMesh[0];
            }
            else if (progressValue <= 90f)
            {
                if (State == BeefState.WellDone)
                {
                    return;
                }
                State = BeefState.WellDone;
                thickBeefRenderer.sharedMaterial = stateMesh[1];
            }
            else
            {
                if (State == BeefState.Burned)
                {
                    return;
                }
                State = BeefState.Burned;
                thickBeefRenderer.sharedMaterial = stateMesh[2];
            }




        }
    }
}



