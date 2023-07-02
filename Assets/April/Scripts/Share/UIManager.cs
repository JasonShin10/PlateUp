using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace April
{
    public class UIManager : SingletonBase<UIManager>
    {
        public static T Show<T>(UIList ui, UnityAction showCallback = null) where T : UIBase
        {
            var newUI = Singleton.GetUI<T>(ui);
            newUI.Show(showCallback);
            return newUI;
        }

        public static T Hide<T>(UIList ui, UnityAction hideCallback = null) where T : UIBase
        {
            var targetUI = Singleton.GetUI<T>(ui);
            targetUI.Hide(hideCallback);
            return targetUI;
        }


        public bool IsInitialized { get; private set; } = false;


        public Dictionary<UIList, UIBase> panels = new Dictionary<UIList, UIBase>();
        public Dictionary<UIList, UIBase> popups = new Dictionary<UIList, UIBase>();

        [SerializeField] private Transform panelRoot;
        [SerializeField] private Transform popupRoot;

        private const string UI_PATH = "UI/Prefab/";


        public void Initialize()
        {
            if (panelRoot == null)
            {
                GameObject goPanelRoot = new GameObject("Panel Root");
                panelRoot = goPanelRoot.transform;
                panelRoot.parent = transform;
                panelRoot.localPosition = Vector3.zero;
                panelRoot.localRotation = Quaternion.identity;
                panelRoot.localScale = Vector3.one;
            }

            if (popupRoot == null)
            {
                GameObject goPanelRoot = new GameObject("Popup Root");
                popupRoot = goPanelRoot.transform;
                popupRoot.parent = transform;
                popupRoot.localPosition = Vector3.zero;
                popupRoot.localRotation = Quaternion.identity;
                popupRoot.localScale = Vector3.one;
            }

            for (int index = (int)UIList.SCENE_PANEL + 1; index < (int)UIList.MAX_SCENE_PANEL; index++)
            {
                panels.Add((UIList)index, null);
            }

            for (int index = (int)UIList.SCENE_POPUP + 1; index < (int)UIList.MAX_SCENE_POPUP; index++)
            {
                popups.Add((UIList)index, null);
            }

            IsInitialized = true;
        }


        public bool GetUI<T>(UIList uiName, out T ui, bool reload = false) where T : UIBase
        {
            ui = GetUI<T>(uiName, reload);
            return ui != null;
        }

        public T GetUI<T>(UIList uiName, bool reload = false) where T : UIBase
        {
            // Get Panel
            if (UIList.SCENE_PANEL < uiName && uiName < UIList.MAX_SCENE_PANEL)
            {
                if (panels.ContainsKey(uiName))
                {
                    if (reload && panels[uiName] != null)
                    {
                        Destroy(panels[uiName].gameObject);
                        panels[uiName] = null;
                    }

                    if (panels[uiName] == null)
                    {
                        string path = UI_PATH + uiName.ToString();
                        GameObject loadedUI = Resources.Load<GameObject>(path) as GameObject;
                        if (loadedUI == null) return null;

                        T result = loadedUI.GetComponent<T>();
                        if (result == null) return null;

                        panels[uiName] = Instantiate(loadedUI, panelRoot).GetComponent<T>() as T;

                        if (panels[uiName]) panels[uiName].gameObject.SetActive(false);
                        return panels[uiName].GetComponent<T>();
                    }
                    else
                    {
                        return panels[uiName].GetComponent<T>();
                    }
                }
            }

            // Get Popup
            if (UIList.SCENE_POPUP < uiName && uiName < UIList.MAX_SCENE_POPUP)
            {
                if (popups.ContainsKey(uiName))
                {
                    if (reload && popups[uiName] != null)
                    {
                        Destroy(popups[uiName].gameObject);
                        popups[uiName] = null;
                    }

                    if (popups[uiName] == null)
                    {
                        string path = UI_PATH + uiName.ToString();
                        GameObject loadedUI = Resources.Load<GameObject>(path) as GameObject;
                        if (loadedUI == null) return null;

                        T result = loadedUI.GetComponent<T>();
                        if (result == null) return null;

                        popups[uiName] = Instantiate(loadedUI, popupRoot).GetComponent<T>() as T;

                        if (popups[uiName]) popups[uiName].gameObject.SetActive(false);
                        return popups[uiName].GetComponent<T>();
                    }
                    else
                    {
                        return popups[uiName].GetComponent<T>();
                    }
                }
            }

            return null;
        }


        public void HideAllUI()
        {
            HideAllPopup();
            HideAllPanel();
        }

        /// <summary> Hide All 2D Popup UI </summary>
        public void HideAllPopup()
        {
            foreach (var popup in popups)
            {
                if (popup.Value)
                {
                    popup.Value.Hide();
                }
            }
        }

        /// <summary> Hide All 2D Panel </summary>
        public void HideAllPanel()
        {
            foreach (var panel in panels)
            {
                if (panel.Value)
                {
                    panel.Value.Hide();
                }
            }
        }

    }
}

