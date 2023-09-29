using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace April
{
    public class TitleUI : UIBase
    {
        public static TitleUI Instance => UIManager.Singleton.GetUI<TitleUI>(UIList.TitleUI);

        public Button start;
        public Button exit;
        public Sprite nonClickImage;
        public Sprite clickImage;
   
        public void OnClickStartButton()
        {
            start.image.sprite = clickImage;
            StartCoroutine(DelayedSceneChange());
        }

        public void OnClickExitButton()
        {
            exit.image.sprite = nonClickImage;
            StartCoroutine(DelayedSceneExit());
        }

        private IEnumerator DelayedSceneChange()
        {
            yield return new WaitForSeconds(0.5f);  
            Main.Singleton.ChangeScene(SceneType.Game);
        }

        private IEnumerator DelayedSceneExit()
        {
            yield return new WaitForSeconds(0.5f);
            Main.Singleton.Quit();
        }
    }
}