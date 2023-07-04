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
                // 1. UIManager.Singleton을 호출했을 때 이 부분이 실행됩니다. _instance.Value를 리턴하게 됩니다.
                return _instance.Value;
            }
        }

        private static readonly Lazy<T> _instance = new Lazy<T>(
            () =>
            {
                // 2. Lazy<T>의 Value 속성을 처음 호출할 때 이 람다 함수가 실행됩니다. 이는 인스턴스를 생성하는 부분입니다.
                T instance = FindObjectOfType(typeof(T)) as T;

                if (instance == null)
                {
                    // 3. 인스턴스가 없다면 새로운 GameObject를 만들고 T타입의 컴포넌트를 추가합니다. 그리고 이를 instance로 설정합니다.
                    GameObject obj = new GameObject(typeof(T).ToString());
                    instance = obj.AddComponent(typeof(T)) as T;
#if UNITY_EDITOR
                    if (EditorApplication.isPlaying)
                    {
                        // 4. Unity의 장면 전환 시에도 해당 GameObject가 파괴되지 않게 설정합니다.
                        // 생성자 안의 DontDestroyOnLoad(obj);는 싱글톤 인스턴스를 만드는 과정에서 새롭게 생성된 GameObject를 씬 변경 시에 파괴되지 않도록 보호합니다. 이 GameObject는 싱글톤 인스턴스를 담는 용도로 사용됩니다.
                        DontDestroyOnLoad(obj);
                    }
#else
                    DontDestroyOnLoad(obj);
#endif
                }
                // 5. 이제 만들어진 인스턴스를 반환합니다. 이 반환된 인스턴스가 _instance.Value가 됩니다.
                return instance;
            });

        protected virtual void Awake()
        {
            // DontDestroyOnLoad(gameObject);는 싱글톤 객체 자체를 씬 변경 시에 파괴되지 않도록 보호합니다. 이것은 실제로 싱글톤 패턴을 사용하는 클래스 (예를 들어, UIManager)의 인스턴스에 적용됩니다.
            DontDestroyOnLoad(gameObject);
        }
    }
}

