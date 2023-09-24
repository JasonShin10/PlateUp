using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Xml.Serialization;
using TMPro;
using UnityEngine.Rendering;

namespace April
{
    public class UpgradeUI : UIBase
    {
        public static UpgradeUI Instance { get; private set; }

        public Button option1;
        public Button option2;
        public TextMeshProUGUI option1Text;
        public TextMeshProUGUI option2Text;

        public Sprite nonClickImage;
        public Sprite clickImage;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            UIManager.Hide<UpgradeUI>(UIList.UpgradeUI);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void OnEnable()
        {
            IngameUpgradeSystem.Instance.PickRandomOption(out string text1, out string text2);
            option1Text.text = text1;
            option2Text.text = text2;
        }

        private void OnDisable()
        {
            
        }

        public void OnClickFunction1()
        {
            option1.image.sprite = clickImage;
            Time.timeScale = 1f;
            StartCoroutine(DelayedFunction1());
        
        }

        private IEnumerator DelayedFunction1()
        {
            yield return new WaitForSeconds(0.5f);
            IngameUpgradeSystem.Instance.buttonFunction1?.Invoke();
            option1.image.sprite = nonClickImage;
            UIManager.Hide<UpgradeUI>(UIList.UpgradeUI);
            IngameTimeSystem.Instance.IsUpdateEnable = true;
        }
        public void OnClickFunction2()
        {
            option2.image.sprite = clickImage;
            Time.timeScale = 1f;
            StartCoroutine(DelayedFunction2());
        }

        private IEnumerator DelayedFunction2()
        {
            yield return new WaitForSeconds(0.5f);
            IngameUpgradeSystem.Instance.buttonFunction2?.Invoke();
            option2.image.sprite = nonClickImage;
            UIManager.Hide<UpgradeUI>(UIList.UpgradeUI);
            IngameTimeSystem.Instance.IsUpdateEnable = true;
        }
    }
}