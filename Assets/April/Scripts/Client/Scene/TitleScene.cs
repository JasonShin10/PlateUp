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
        {            
            var async = SceneManager.LoadSceneAsync(SceneType.Title.ToString(), IsAdditiveScene ? LoadSceneMode.Additive : LoadSceneMode.Single);
            yield return new WaitUntil(() => async.isDone);

            //UIManager.Show<TitleUI>(UIList.TitleUI);
            UIManager.Show<DummyUI>(UIList.DummyUI);
        }

        public override IEnumerator OnEnd()
        {
            yield return null;
        }
    }
}

