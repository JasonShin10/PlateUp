using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class IngameTimeSystem : MonoBehaviour
    {
        public static IngameTimeSystem Instance { get; private set; }

        public bool IsUpdateEnable { get => isUpdateEnable; set=> isUpdateEnable = value; }
        public float TimeSpeedRate { get => timeSpeedRate; set => timeSpeedRate = value; }

        private float ingameSecondsPerOneDay = 30f;
        public float countDownTime = 200;
        private float progressTime = 0;
        private bool isUpdateEnable = true;
        private float timeSpeedRate = 1f;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Update()
        {
            if (!isUpdateEnable)
                return;

            progressTime += (Time.deltaTime * timeSpeedRate);
            var ingameUI = UIManager.Singleton.GetUI<IngameUI>(UIList.IngameUI);
            ingameUI.SetTimeText(progressTime);
            ingameUI.ActivateClearUI();

            if (progressTime > ingameSecondsPerOneDay)
            {
                // To do : Change to Next Day Progress ?
                // To do : Game Stop ?

                //IsUpdateEnable = false;
                IngameDaySystem.Instance.SetDay();
                IngameCustomerFactorySystem.Instance.MakeSpawnFast();
                UIManager.Show<UpgradeUI>(UIList.UpgradeUI);

                IsUpdateEnable = false;
                IngameCustomerFactorySystem.Instance.enabled = false;

                Time.timeScale = 0f;

                ingameSecondsPerOneDay += 60f;
            }
        }

        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
        }
    }
}

