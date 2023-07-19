using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace April
{
    public class IngameUI : UIBase
    {
        public TextMeshProUGUI timeText;
        private float elapsedTime = 0f;

        private void Update()
        {
            elapsedTime += Time.deltaTime;

            int hours = (int)(elapsedTime / 3600);
            int minutes = (int)((elapsedTime % 3600) / 60);
            int seconds = (int)(elapsedTime % 60);

            timeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
       
    }
}