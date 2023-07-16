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
        public Toggle ActionToggle => toggle;

        [SerializeField] private InteractionUI parentUI;
        [SerializeField] private Toggle toggle;
        [SerializeField] private TextMeshProUGUI buttonText;

        private Action callback;

        private void Awake()
        {
            toggle.onValueChanged.AddListener(OnToggle);
        }

        void OnToggle(bool isOn)
        {
            if (isOn)
            {
                OnExecuteAction();
                parentUI.SetSelectedItem(this);
            }
        }

        public void Init(string actionName, Action callback)
        {
            buttonText.text = actionName;
            this.callback = callback;
        }

        public void OnExecuteAction()
        {
            this.callback?.Invoke();
            parentUI.CloseActionUI();
        }
    }
}

