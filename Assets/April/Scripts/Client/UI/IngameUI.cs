using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace April
{
    public class IngameUI : UIBase
    {
        public static IngameUI Instance => UIManager.Singleton.GetUI<IngameUI>(UIList.IngameUI);

        //public TextMeshProUGUI timeText;
        public TextMeshProUGUI totalAssets;
        public TextMeshProUGUI currentStage;
        public Slider timeSlider;
        public Image[] hearts;
        private int totalAssetsAmount = 0;

        private void Start()
        {
            SetDayText(0);
            timeSlider.maxValue = 60;
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
            //int hours = (int)(elapsedTime / 3600);
            //int minutes = (int)((elapsedTime % 3600) / 60);
            //int seconds = (int)(elapsedTime % 60);

            //timeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            timeSlider.value = elapsedTime;
            if(timeSlider.value >= timeSlider.maxValue)
            {
                ClearUI.Instance.CaculateScore(totalAssetsAmount);
            }
        }

        public void SetDayText(int dayNumber)
        {
            currentStage.text = string.Format("Day{0}", dayNumber);
        }

        public void MakeSpeedFast()
        {
            IngameTimeSystem.Instance.SetTimeScale(Time.timeScale * 2f);
        }

        public void SetLife(int remainLife)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].enabled = i < remainLife;
            }
        }
    }

}