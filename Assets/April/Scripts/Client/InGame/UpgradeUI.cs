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
            Time.timeScale = 1f;
        }

        public void TriggerOption1Function()
        {
            IngameUpgradeSystem.Instance.buttonFunction1?.Invoke();
            UIManager.Hide<UpgradeUI>(UIList.UpgradeUI);
            IngameTimeSystem.Instance.IsUpdateEnable = true;
        }

        public void TriggerOption2Function()
        {
            IngameUpgradeSystem.Instance.buttonFunction2?.Invoke();
            UIManager.Hide<UpgradeUI>(UIList.UpgradeUI);
            IngameTimeSystem.Instance.IsUpdateEnable = true;
        }
    }
}