using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class InteractActionData
    {
        public string actionName;
        public Action callback;
    }

    public class InteractionUI : UIBase
    {
        [SerializeField] private Transform actionItemRoot;
        [SerializeField] private InteractionUI_ActionItem actionItemPrefab;

        private List<InteractionUI_ActionItem> createdActionItems = new List<InteractionUI_ActionItem>();

        private void Awake()
        {
            actionItemPrefab.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Disable();
            InputManager.Singleton.InputMaster.Act.Enable();
            InputManager.Singleton.InputMaster.Act.InteractionShortcut.performed += InteractionShortcut_performed;
        }

        private void OnDisable()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Enable();
            InputManager.Singleton.InputMaster.Act.Disable();
            InputManager.Singleton.InputMaster.Act.InteractionShortcut.performed -= InteractionShortcut_performed;
        }

        private void InteractionShortcut_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            createdActionItems[0].OnExecuteAction();
            CloseActionUI();
        }

        public void InitActions(List<InteractActionData> actions)
        {
            ClearActions();
            for (int i = 0; i < actions.Count; i++)
            {
                var newActionItem = Instantiate(actionItemPrefab, actionItemRoot);
                newActionItem.Init(actions[i].actionName, actions[i].callback);
                newActionItem.gameObject.SetActive(true);

                createdActionItems.Add(newActionItem);
            }

            var closeActionItem = Instantiate(actionItemPrefab, actionItemRoot);
            closeActionItem.Init("Close UI", CloseActionUI);
            closeActionItem.gameObject.SetActive(true);

            createdActionItems.Add(closeActionItem);
        }

        public void ClearActions()
        {
            createdActionItems.ForEach(x => Destroy(x.gameObject));
            createdActionItems.Clear();
        }

        public void CloseActionUI()
        {
            UIManager.Hide<InteractionUI>(UIList.InteractionUI);
        }
    }
}