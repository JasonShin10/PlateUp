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
            // UIManager.Singleton.Initialize()�� UIManagerŬ������ Singleton �Ӽ��� ���� �̱��� �ν��Ͻ��� ��������, �� �ν��Ͻ��� Initialize �޼��带 ȣ���ϰڴٴ� �ǹ��Դϴ�.

            // 1.. UIManager Ŭ������ �̱��� �ν��Ͻ��� Initialize �޼��带 ȣ���մϴ�. 
            // UIManager.Singleton�� �̱��� �ν��Ͻ��� �����ϰ�, Initialize()�� �����ŵ�ϴ�.
            UIManager.Singleton.Initialize();

            // 2.. ChangeScene �޼��带 ȣ���Ͽ� Title ������ �̵��մϴ�. 
            // �̰��� �ʱ�ȭ�� �Ϸ�� �� ������ ù ��° ���� �ε��ϴ� ���� �����մϴ�.
            ChangeScene(SceneType.Title);
        }

        public void ChangeScene(SceneType sceneType, Action sceneLoadCallback = null)
        {
            // 3.. ���� ���� Ÿ���� ��û���� ���� Ÿ�԰� ������ ���, �� �̹� �ش� ���� ���� ���, �޼��带 �����մϴ�.
            if (currentSceneType == sceneType)
                return;
            //4
            // 4.. ��û���� ���� Ÿ�Կ� ���� �ٸ� ���� �ε��մϴ�. 
            // �� �������� Title ���� ó��������, ���� ���ӿ����� ���� ���� ó���� �� �ֽ��ϴ�.
            switch (sceneType)
            {
                case SceneType.Title:
                    {
                        // 5.. Title ���� �ε��մϴ�. TitleScene�� ���� ��ü���� Ÿ���� ��Ÿ���ϴ�. 
                        // �̸� ���� ���� Ÿ�Կ� ���� �ٸ� ������ ������ �� �ֽ��ϴ�.
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

        // where Ű���带 ����ϸ� ���׸� Ŭ������ �޼��尡 Ư�� Ÿ�Ը� ó���ϵ��� ������ �� �ִ�. �Ʒ������� 'T'�� �ݵ�� SceneBaseŸ���� ��ӹ޾ƾ� �Ѵ�.
        private void ChangeScene<T>(SceneType sceneType, Action sceneLoadedCallback = null) where T : SceneBase
        {
            //5
            // 6.. �� ������ ���� ���̸� �޼��带 �����մϴ�. ���ÿ� �� �� �̻� �� ������ �Ͼ�� �ʵ��� �մϴ�.
            if (IsOnProgressSceneChange)
            {
                return;
            }
            //6
            // 7.. ���� �񵿱������� �����ϴ� �ڷ�ƾ�� �����մϴ�. ���⼭ ���׸� Ÿ�� T�� ���� ������ ��Ÿ���ϴ�.
            StartCoroutine(ChangeSceneAsync<T>(sceneType, sceneLoadedCallback));
        }

        private IEnumerator ChangeSceneAsync<T>(SceneType sceneType, Action sceneLoadedCallback = null) where T : SceneBase
        {
            //7
            // 8.. �� ������ ���۵Ǿ����� ��Ÿ���� �÷��׸� �����մϴ�.
            IsOnProgressSceneChange = true;


            UIManager.Singleton.HideAllUI();
            // 9.. ���� ���� �����ϸ�, ���� ���� ������ �����ϰ� ���� �ı��մϴ�.
            if (CurrentScene)
            {
                yield return StartCoroutine(CurrentScene.OnEnd());
                Destroy(CurrentScene.gameObject);
            }

            // 10.. Empty ���� �񵿱������� �ε��մϴ�.
            var asnyc = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneType.Empty.ToString());
            yield return new WaitUntil(() => { return asnyc.isDone; });

            // 11.. ���ο� �� ��ü�� �����ϰ�, ���׸� Ÿ�� T�� �� ������Ʈ�� �߰��մϴ�. �̸� ���� ���ο� ���� ���� ������ �غ��մϴ�.
            GameObject sceneGo = new GameObject(typeof(T).Name);
            sceneGo.transform.parent = transform;
            currentScene = sceneGo.AddComponent<T>();
            currentSceneType = sceneType;

            // 12..���ο� ���� ���� ������ �����մϴ�.
            yield return StartCoroutine(currentScene.OnStart());

            // 13.. �� ������ �Ϸ�Ǿ����� ��Ÿ���� �÷��׸� �����մϴ�.
            IsOnProgressSceneChange = false;
            sceneLoadedCallback?.Invoke();
        }
    }
}

