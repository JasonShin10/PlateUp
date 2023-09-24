using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
namespace April
{
    public class ClearUI : UIBase
    {
        public static ClearUI Instance {get; private set; }

        public List<Image> starImages = new List<Image>();
        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }
        // Start is called before the first frame update
        void Start()
        {
            UIManager.Hide<ClearUI>(UIList.ClearUI);
        }

        public void CaculateScore(int assets)
        {

            
            if (assets >= 300)
            {
                for (int i = 0; i< 1; i++)
                {
                    starImages[i].gameObject.SetActive(false);
                }
            }
            else if (assets >= 200)
            {
                for (int i = 0; i < 2; i++)
                {
                    starImages[i].gameObject.SetActive(false);
                }
            }
            else if (assets >= 100)
            {
                for (int i = 0; i < 3; i++)
                {
                    starImages[i].gameObject.SetActive(false);
                }
            }
            else
            {
                Console.WriteLine("Score is less than 100.");
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
