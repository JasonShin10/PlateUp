using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace April
{
    public class InteractionUI_ActionItem : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI buttonText;

        private Action callback;

        public void Init(string actionName, Action callback)
        {
            buttonText.text = actionName;

            this.callback = callback;
            button.onClick.AddListener(OnExecuteAction);
        }

        public void OnExecuteAction()
        {
            this.callback?.Invoke();
        }
    }
}

