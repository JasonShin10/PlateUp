using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class IngameUI : UIBase
    {
        public void OnClickExitButton()
        {
            Main.Singleton.ChangeScene(SceneType.Title);
        }
    }
}