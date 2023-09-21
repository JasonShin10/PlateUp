using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace April
{
    public class TitleScene : SceneBase
    {
        public override bool IsAdditiveScene => false;

        public override IEnumerator OnStart()
        {       //9   
            var async = SceneManager.LoadSceneAsync(SceneType.Title.ToString(), IsAdditiveScene ? LoadSceneMode.Additive : LoadSceneMode.Single);
            yield return new WaitUntil(() => async.isDone);

            //UIManager.Show<TitleUI>(UIList.TitleUI);
            //10
            UIManager.Show<TitleUI>(UIList.TitleUI);
        }

        public override IEnumerator OnEnd()
        {
            yield return null;
        }
    }
}

