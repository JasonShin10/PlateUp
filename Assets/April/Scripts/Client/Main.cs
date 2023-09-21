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

        //public SceneType CurrentSceneType
        //{
        //    get { return currentSceneType; }
        //}
        public SceneType CurrentSceneType => currentSceneType;

        [SerializeField] private SceneType currentSceneType = SceneType.None;

        public void Initialize()
        {
            // UI System Load & Initialize
            //3
            // UIManager.Singleton.Initialize()는 UIManager클래스의 Singleton 속성을 통해 싱글턴 인스턴스를 가져오고, 그 인스턴스의 Initialize 메서드를 호출하겠다는 의미입니다.

            // 1.. UIManager 클래스의 싱글턴 인스턴스의 Initialize 메서드를 호출합니다. 
            // UIManager.Singleton이 싱글턴 인스턴스를 참조하고, Initialize()를 실행시킵니다.
            UIManager.Singleton.Initialize();

            // 2.. ChangeScene 메서드를 호출하여 Title 씬으로 이동합니다. 
            // 이것은 초기화가 완료된 후 게임의 첫 번째 씬을 로드하는 것을 시작합니다.
            ChangeScene(SceneType.Title);
        }

        public void ChangeScene(SceneType sceneType, Action sceneLoadCallback = null)
        {
            // 3.. 현재 씬의 타입이 요청받은 씬의 타입과 동일한 경우, 즉 이미 해당 씬에 있을 경우, 메서드를 종료합니다.
            if (currentSceneType == sceneType)
                return;
            //4
            // 4.. 요청받은 씬의 타입에 따라 다른 씬을 로드합니다. 
            // 이 예에서는 Title 씬만 처리하지만, 실제 게임에서는 여러 씬을 처리할 수 있습니다.
            switch (sceneType)
            {
                case SceneType.Title:
                    {
                        // 5.. Title 씬을 로드합니다. TitleScene은 씬의 구체적인 타입을 나타냅니다. 
                        // 이를 통해 씬의 타입에 따라 다른 로직을 실행할 수 있습니다.
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
            //1
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            //2
            Initialize();
        }

        private void OnDestroy()
        {
            Singleton = null;
        }

        // where 키워드를 사용하면 제네릭 클래스나 메서드가 특정 타입만 처리하도록 제한할 수 있다. 아래에서는 'T'가 반드시 SceneBase타입을 상속받아야 한다.
        private void ChangeScene<T>(SceneType sceneType, Action sceneLoadedCallback = null) where T : SceneBase
        {
            //5
            // 6.. 씬 변경이 진행 중이면 메서드를 종료합니다. 동시에 두 번 이상 씬 변경이 일어나지 않도록 합니다.
            if (IsOnProgressSceneChange)
            {
                return;
            }
            //6
            // 7.. 씬을 비동기적으로 변경하는 코루틴을 시작합니다. 여기서 제네릭 타입 T는 씬의 종류를 나타냅니다.
            StartCoroutine(ChangeSceneAsync<T>(sceneType, sceneLoadedCallback));
        }

        private IEnumerator ChangeSceneAsync<T>(SceneType sceneType, Action sceneLoadedCallback = null) where T : SceneBase
        {
            //7
            // 8.. 씬 변경이 시작되었음을 나타내는 플래그를 설정합니다.
            IsOnProgressSceneChange = true;


            UIManager.Singleton.HideAllUI();
            // 9.. 현재 씬이 존재하면, 씬의 종료 동작을 실행하고 씬을 파괴합니다.
            if (CurrentScene)
            {
                yield return StartCoroutine(CurrentScene.OnEnd());
                Destroy(CurrentScene.gameObject);
            }

            // 10.. Empty 씬을 비동기적으로 로드합니다.
            var asnyc = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneType.Empty.ToString());
            yield return new WaitUntil(() => { return asnyc.isDone; });

            // 11.. 새로운 씬 객체를 생성하고, 제네릭 타입 T의 씬 컴포넌트를 추가합니다. 이를 통해 새로운 씬의 시작 동작을 준비합니다.
            GameObject sceneGo = new GameObject(typeof(T).Name);
            sceneGo.transform.parent = transform;
            currentScene = sceneGo.AddComponent<T>();
            currentSceneType = sceneType;

            // 12..새로운 씬의 시작 동작을 실행합니다.
            yield return StartCoroutine(currentScene.OnStart());

            // 13.. 씬 변경이 완료되었음을 나타내는 플래그를 해제합니다.
            IsOnProgressSceneChange = false;
            sceneLoadedCallback?.Invoke();
        }
    }
}

