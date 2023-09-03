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

        private const float IngameSecondsPerOneDay = 60f;

        private float progressTime = 0;
        private bool isUpdateEnable = true;
        private float timeSpeedRate = 1.0f;

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

            if (progressTime > IngameSecondsPerOneDay)
            {
                // To do : Change to Next Day Progress ?
                // To do : Game Stop ?

                progressTime = 0f;
                IsUpdateEnable = false;
            }
        }

        [Button("Change Time Speed")]
        public void SetTimeSpeed(float speed)
        {
            TimeSpeedRate = speed;
            //Time.timeScale = speed;
        }
    }
}

