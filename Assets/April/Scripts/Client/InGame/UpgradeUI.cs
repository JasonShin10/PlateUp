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
        public List<Action> buttonFunctions;
        public Action buttonFunction1;
        public Action buttonFunction2;
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
            buttonFunctions = new List<Action> { SpawnWaitress, UpgradeStove, IncreasePatience, IncreaseSpeed };
            PickRandomOption();
        }
        public void PickRandomOption()
        {
            int random1 = UnityEngine.Random.Range(0,buttonFunctions.Count);
            buttonFunction1 = buttonFunctions[random1];
            option1Text.text = GetDescription(buttonFunctions[random1]);
            buttonFunctions.Remove(buttonFunctions[random1]);
            int random2 = UnityEngine.Random.Range(0, buttonFunctions.Count);
            buttonFunction2 = buttonFunctions[random2];
            option2Text.text = GetDescription(buttonFunctions[random2]);
            
        }

        private void SpawnWaitress()
        {
            Debug.Log("SpawnWaitress");
            
        }

        private void UpgradeStove()
        {
            Debug.Log("UpgradeStove");
            
        }

        private void IncreasePatience()
        {
            Debug.Log("IncreasePatience");
        }

        private void IncreaseSpeed()
        {
            Debug.Log("IncreaseSpeed");
            
        }
        private string GetDescription(Action action)
        {
            if (action == SpawnWaitress)
                return "Spawn Waitress";
            if (action == UpgradeStove)
                return "Upgrade Stove";
            if (action == IncreasePatience)
                return "Increase Patience";
            if (action == IncreaseSpeed)
                return "Increase Speed";



            return "";
        }
        public void TriggerOption1Function()
        {
            buttonFunction1?.Invoke();
            UIManager.Hide<UpgradeUI>(UIList.UpgradeUI);
            IngameTimeSystem.Instance.IsUpdateEnable = true;
        }

        public void TriggerOption2Function()
        {
            buttonFunction1?.Invoke();
            UIManager.Hide<UpgradeUI>(UIList.UpgradeUI);
            IngameTimeSystem.Instance.IsUpdateEnable = true;
        }
    }
}