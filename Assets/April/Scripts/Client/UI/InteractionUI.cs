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
        private InteractionUI_ActionItem currentSelectedItem = null;
        private int currentSelectedItemIndex = 0;

        private void Awake()
        {
            actionItemPrefab.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Disable();
            InputManager.Singleton.InputMaster.UIControl.Enable();
            InputManager.Singleton.InputMaster.UIControl.SelectionUp.performed += SelectionUp;
            InputManager.Singleton.InputMaster.UIControl.SelectionDown.performed += SelectionDown;
            InputManager.Singleton.InputMaster.UIControl.Select.performed += Select;
        }

        private void OnDisable()
        {
            InputManager.Singleton.InputMaster.PlayerControl.Enable();
            InputManager.Singleton.InputMaster.UIControl.Disable();
            InputManager.Singleton.InputMaster.UIControl.Select.performed -= Select;
        }

        private void SelectionUp(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            currentSelectedItemIndex--;
            currentSelectedItemIndex = Mathf.Clamp(currentSelectedItemIndex, 0, createdActionItems.Count - 1);

            createdActionItems[currentSelectedItemIndex].ActionToggle.SetIsOnWithoutNotify(true);
            currentSelectedItem = createdActionItems[currentSelectedItemIndex];
        }

        private void SelectionDown(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            currentSelectedItemIndex++;
            currentSelectedItemIndex = Mathf.Clamp(currentSelectedItemIndex, 0, createdActionItems.Count - 1);

            createdActionItems[currentSelectedItemIndex].ActionToggle.SetIsOnWithoutNotify(true);
            currentSelectedItem = createdActionItems[currentSelectedItemIndex];
        }

        private void Select(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            currentSelectedItem?.OnExecuteAction();
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
            createdActionItems[0].ActionToggle.SetIsOnWithoutNotify(true);
            currentSelectedItemIndex = 0;
            currentSelectedItem = createdActionItems[0];
        }

        public void ClearActions()
        {
            currentSelectedItem = null;
            currentSelectedItemIndex = 0;

            createdActionItems.ForEach(x => Destroy(x.gameObject));
            createdActionItems.Clear();
        }

        public void CloseActionUI()
        {
            UIManager.Hide<InteractionUI>(UIList.InteractionUI);
        }

        public void SetSelectedItem(InteractionUI_ActionItem selectedItem)
        {
            currentSelectedItem = selectedItem;
        }
    }
}