using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class TitleUI : UIBase
    {
        public void OnClickStartButton()
        {
            Main.Singleton.ChangeScene(SceneType.Game);
        }

        public void OnClickExitButton()
        {
            Main.Singleton.Quit();
        }
    }
}