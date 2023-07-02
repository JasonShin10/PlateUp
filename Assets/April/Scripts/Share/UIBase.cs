using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace April
{
    public class UIBase : MonoBehaviour
    {
        public virtual void Show(UnityAction callback = null)
        {
            gameObject.SetActive(true);
            callback?.Invoke();
        }

        public virtual void Hide(UnityAction callback = null)
        {
            gameObject.SetActive(false);
            callback?.Invoke();
        }
    }
}
