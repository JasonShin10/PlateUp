using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace April
{

    public class GameOverUI : UIBase
    {
        public static GameOverUI Instance => UIManager.Singleton.GetUI<GameOverUI>(UIList.GameOverUI);

        public Button restart;
        public Button exit;
        public Sprite nonClickImage;
        public Sprite clickImage;

        void Start()
        {
            UIManager.Hide<GameOverUI>(UIList.GameOverUI);
        }

        public void OnRestart()
        {
            restart.image.sprite = clickImage;
            StartCoroutine(DelayedLoadScene());
        }
        private IEnumerator DelayedLoadScene()
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            UIManager.Hide<GameOverUI>(UIList.GameOverUI);
        }

        private IEnumerator DelayedOnExit()
        {
            yield return new WaitForSeconds(0.5f);
            Application.Quit();
            UIManager.Hide<GameOverUI>(UIList.GameOverUI);
        }
        public void OnExit()
        {
            exit.image.sprite = clickImage;
            StartCoroutine(DelayedOnExit());
            UIManager.Hide<GameOverUI>(UIList.GameOverUI);
        }
    }
}