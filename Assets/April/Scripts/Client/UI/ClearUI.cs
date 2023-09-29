using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
namespace April
{
    public class ClearUI : UIBase
    {
        public static ClearUI Instance => UIManager.Singleton.GetUI<ClearUI>(UIList.ClearUI);

        public List<Image> starImages = new List<Image>();

        public Button restart;
        public Button exit;
        public Sprite nonClickImage;
        public Sprite clickImage;

        // Start is called before the first frame update
        void Start()
        {
            UIManager.Hide<ClearUI>(UIList.ClearUI);
        }

        public void CaculateScore(int assets)
        {
            UIManager.Show<ClearUI>(UIList.ClearUI);
 
            if (assets >= 800)
            {
                for (int i = 0; i < 3; i++)
                {
                    starImages[i].gameObject.SetActive(true);
                }
            }
            else if (assets >= 500)
            {
                for (int i = 0; i < 2; i++)
                {
                    starImages[i].gameObject.SetActive(true);
                }
            }
            else if (assets >= 200)
            {
                for (int i =0; i< 1; i++)
                {
                    starImages[i].gameObject.SetActive(true);
                }
            }

        }

        public void OnRestart()
        {
            restart.image.sprite = clickImage;
            StartCoroutine(DelayedLoadScene());
        }
        private IEnumerator DelayedLoadScene()
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            UIManager.Hide<ClearUI>(UIList.ClearUI);
        }

        private IEnumerator DelayedOnExit()
        {
            yield return new WaitForSeconds(0.5f);
            Application.Quit();
            UIManager.Hide<ClearUI>(UIList.ClearUI);
        }
        public void OnExit()
        {
            exit.image.sprite = clickImage;
            StartCoroutine(DelayedOnExit());
            UIManager.Hide<ClearUI>(UIList.ClearUI);
        }


    }
}
