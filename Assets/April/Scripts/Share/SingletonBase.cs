using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace April
{
    public class SingletonBase<T> : MonoBehaviour where T : class
    {
        public static T Singleton
        {
            get
            {
                // 1. UIManager.Singleton�� ȣ������ �� �� �κ��� ����˴ϴ�. _instance.Value�� �����ϰ� �˴ϴ�.
                return _instance.Value;
            }
        }

        private static readonly Lazy<T> _instance = new Lazy<T>(
            () =>
            {
                // 2. Lazy<T>�� Value �Ӽ��� ó�� ȣ���� �� �� ���� �Լ��� ����˴ϴ�. �̴� �ν��Ͻ��� �����ϴ� �κ��Դϴ�.
                T instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    // 3. �ν��Ͻ��� ���ٸ� ���ο� GameObject�� ����� TŸ���� ������Ʈ�� �߰��մϴ�. �׸��� �̸� instance�� �����մϴ�.
                    GameObject obj = new GameObject(typeof(T).ToString());
                    instance = obj.AddComponent(typeof(T)) as T;
#if UNITY_EDITOR
                    if (EditorApplication.isPlaying)
                    {
                        // 4. Unity�� ��� ��ȯ �ÿ��� �ش� GameObject�� �ı����� �ʰ� �����մϴ�.
                        // ������ ���� DontDestroyOnLoad(obj);�� �̱��� �ν��Ͻ��� ����� �������� ���Ӱ� ������ GameObject�� �� ���� �ÿ� �ı����� �ʵ��� ��ȣ�մϴ�. �� GameObject�� �̱��� �ν��Ͻ��� ��� �뵵�� ���˴ϴ�.
                        DontDestroyOnLoad(obj);
                    }
#else
                    DontDestroyOnLoad(obj);
#endif
                }
                // 5. ���� ������� �ν��Ͻ��� ��ȯ�մϴ�. �� ��ȯ�� �ν��Ͻ��� _instance.Value�� �˴ϴ�.
                return instance;
            });

        protected virtual void Awake()
        {
            // DontDestroyOnLoad(gameObject);�� �̱��� ��ü ��ü�� �� ���� �ÿ� �ı����� �ʵ��� ��ȣ�մϴ�. �̰��� ������ �̱��� ������ ����ϴ� Ŭ���� (���� ���, UIManager)�� �ν��Ͻ��� ����˴ϴ�.
            DontDestroyOnLoad(gameObject);
        }
    }
}

