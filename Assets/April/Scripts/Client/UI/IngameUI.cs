using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace April
{
    public class IngameUI : UIBase
    {
        public static IngameUI Instance { get; private set; }

        public TextMeshProUGUI timeText;
        public TextMeshProUGUI totalAssets;
        public TextMeshProUGUI currentStage;

        private int totalAssetsAmount = 0;

        private void Awake()
        {
            Instance = this;
           
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void Start()
        {
            SetDayText(0);
        }

        public int TotalAssets
        {
            get
            {
                return totalAssetsAmount;
            }
            set
            {
                totalAssetsAmount = value;
                totalAssets.text = string.Format("Money : {0:###,###,##0}", totalAssetsAmount);
            }
        }

        public void AddAssets(int amount)
        {
            TotalAssets += amount;
        }

        public void SetTimeText(float elapsedTime)
        {
            int hours = (int)(elapsedTime / 3600);
            int minutes = (int)((elapsedTime % 3600) / 60);
            int seconds = (int)(elapsedTime % 60);

            timeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }

        public void SetDayText(int dayNumber)
        {
            currentStage.text = string.Format("Day{0}", dayNumber);
        }
    }


}