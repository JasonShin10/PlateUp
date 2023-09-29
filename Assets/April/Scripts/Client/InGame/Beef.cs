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
        private Renderer beefRenderer;

        public List<Material> stateMesh = new List<Material> ();

        public enum BeefState
        {
            Frozen,
            Raw,
            Medium,
            WellDone,
            Burned
        }

        public Slider slider;

        private void Start()
        {
            beefRenderer = GetComponentInChildren<Renderer>(true);
            slider.maxValue = 90f;
            State = BeefState.Frozen;
        }
        public override int CookingState => (int)State;

        //public override string CookingState
        //{
        //    get
        //    {
        //        return State.ToString();
        //    }
        //}
        [SerializeField] private BeefState state;

        public BeefState State
        {
            get { return state;}
            private set { state = value; }
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
                if (State == BeefState.Raw)
                {
                    return;
                }
                State =  BeefState.Raw;
                beefRenderer.sharedMaterial = stateMesh[0];
            }
            else if (progressValue >= 40f && progressValue < 60f)
            {
                if (State == BeefState.Medium)
                {
                    return;
                }
                State = BeefState.Medium;
                beefRenderer.sharedMaterial = stateMesh[1];
                
            }
            else if (progressValue >= 60f && progressValue < 80f)
            {
                if (State == BeefState.WellDone)
                {
                    return;
                }
                State =BeefState.WellDone;
                beefRenderer.sharedMaterial = stateMesh[2];
            }
            else if (progressValue >= slider.maxValue)
            {
                if (State == BeefState.Burned)
                {
                    return;
                }
                State = BeefState.Burned;
                beefRenderer.sharedMaterial = stateMesh[3];
                slider.gameObject.SetActive(false);
            }
        }
    }

}

