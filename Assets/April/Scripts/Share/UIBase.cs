using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace April
{
    public class UIBase : MonoBehaviour
    {
        [field: SerializeField] public bool IsMouseCursorVisible { get; private set; } = false;

        public virtual void Show(UnityAction callback = null)
        {
            gameObject.SetActive(true);
            callback?.Invoke();
        }

        public virtual void Hide(UnityAction callback = null)
        {
            callback?.Invoke();
            gameObject.SetActive(false);
        }
    }
}

