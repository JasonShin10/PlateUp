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
            Raw,
            Medium,
            WellDone,
            Burned
        }

        public Slider slider;
      

        private void Start()
        {
            beefRenderer = GetComponentInChildren<Renderer>(true);

            slider = GetComponentInChildren<Slider>(true);
            slider.maxValue = 90f;         
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
                State =  BeefState.Medium;
                beefRenderer.sharedMaterial = stateMesh[0];
            }
            else if (progressValue <= 90f)
            {
                if (State == BeefState.WellDone)
                {
                    return;
                }
                State = BeefState.WellDone;
                beefRenderer.sharedMaterial = stateMesh[1];
            }
            else
            {
                if (State == BeefState.Burned)
                {
                    return;
                }
                State =BeefState.Burned;
                beefRenderer.sharedMaterial = stateMesh[2];
            }

            

         
        }
    }

}

