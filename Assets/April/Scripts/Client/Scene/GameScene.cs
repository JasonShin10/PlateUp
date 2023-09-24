using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace April
{
    public class GameScene : SceneBase
    {
        public override bool IsAdditiveScene => false;
        public override IEnumerator OnStart()
        {
            var async = SceneManager.LoadSceneAsync(SceneType.Game.ToString(), IsAdditiveScene ? LoadSceneMode.Additive : LoadSceneMode.Single);
            yield return new WaitUntil(() => async.isDone);

            UIManager.Show<IngameUI>(UIList.IngameUI);
            UIManager.Show<UpgradeUI>(UIList.UpgradeUI);
            UIManager.Show<GameOverUI>(UIList.GameOverUI);
            UIManager.Show<ClearUI>(UIList.ClearUI);
        }

        public override IEnumerator OnEnd()
        {
            yield return null;
        }


    }
}
