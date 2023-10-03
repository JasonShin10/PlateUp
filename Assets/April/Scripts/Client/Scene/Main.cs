using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace April
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private SceneType currentSceneType = SceneType.None;

        public void Initialize()
        {          
            UIManager.Singleton.Initialize();
            ChangeScene(SceneType.Title);
        }

        public void ChangeScene(SceneType sceneType, Action sceneLoadCallback = null)
        {
            if (currentSceneType == sceneType)
                return;

            switch (sceneType)
            {
                case SceneType.Title:
                    {
                        ChangeScene<TitleScene>(SceneType.Title, sceneLoadCallback);
                    }
                    break;
                case SceneType.Game:
                    {
                        ChangeScene<GameScene>(SceneType.Game, sceneLoadCallback);
                    }
                    break;
            }
        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public static Main Singleton { get; private set; } = null;

        public bool IsOnProgressSceneChange { get; private set; }
        public SceneBase CurrentScene => currentScene;

        private SceneBase currentScene;

        private void Awake()
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Singleton = null;
        }
        private void ChangeScene<T>(SceneType sceneType, Action sceneLoadedCallback = null) where T : SceneBase
        {
            if (IsOnProgressSceneChange)
            {
                return;
            }
            StartCoroutine(ChangeSceneAsync<T>(sceneType, sceneLoadedCallback));
        }

        private IEnumerator ChangeSceneAsync<T>(SceneType sceneType, Action sceneLoadedCallback = null) where T : SceneBase
        {
            IsOnProgressSceneChange = true;

            UIManager.Singleton.HideAllUI();
            if (CurrentScene)
            {
                yield return StartCoroutine(CurrentScene.OnEnd());
                Destroy(CurrentScene.gameObject);
            }
            var asnyc = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneType.Empty.ToString());
            yield return new WaitUntil(() => { return asnyc.isDone; });

            GameObject sceneGo = new GameObject(typeof(T).Name);
            sceneGo.transform.parent = transform;
            currentScene = sceneGo.AddComponent<T>();
            currentSceneType = sceneType;

            yield return StartCoroutine(currentScene.OnStart());

            IsOnProgressSceneChange = false;
            sceneLoadedCallback?.Invoke();
        }
    }
}

