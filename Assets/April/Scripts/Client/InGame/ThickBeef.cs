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
            Frozen,
            Raw,
            Medium,
            WellDone,
            Burned
        }
        public Slider slider;
        [SerializeField] private ThickBeefState state;
        public ThickBeefState CurrentThickBeefState { get; set; } = ThickBeefState.Raw;

        public ThickBeefState State
        {
            get { return state; }
            private set { state = value; }
        }

        public override int CookingState => (int)State;
        private void Start()
        {
            thickBeefRenderer = GetComponentInChildren<Renderer>(true);
            slider.maxValue = 90f;
            State = ThickBeefState.Frozen;
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

            if (progressValue >= 20f && progressValue < 40f)
            {
                if (State == ThickBeefState.Raw)
                {
                    return;
                }
                State = ThickBeefState.Raw;
                thickBeefRenderer.sharedMaterial = stateMesh[0];
            }
            else if (progressValue >= 40f && progressValue < 60f)
            {
                if (State == ThickBeefState.Medium)
                {
                    return;
                }
                State = ThickBeefState.Medium;
                thickBeefRenderer.sharedMaterial = stateMesh[1];
            }
            else if (progressValue >= 60f && progressValue < 80f)
            {
                if (State == ThickBeefState.WellDone)
                {
                    return;
                }
                State = ThickBeefState.WellDone;
                thickBeefRenderer.sharedMaterial = stateMesh[2];
            }
            else if (progressValue >= slider.maxValue)
            {
                if (State == ThickBeefState.Burned)
                {
                    return;
                }
                State = ThickBeefState.Burned;
                thickBeefRenderer.sharedMaterial = stateMesh[3];
                slider.gameObject.SetActive(false);
            }


        }
    }
}



