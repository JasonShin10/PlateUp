using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace April
{
    public class Dish : InteractionItem
    {
        public List<Food> ContainedFoodItems => mergedFoodList;


        public Renderer dishRenderer;
        public bool dirty;

        public Slider slider;
        public float progressValue;

        public Transform itemMergeRoot;
        public List<Food> mergedFoodList = new List<Food>();
        public float offset= 0.5f;

        public void AddItem(Food item, Vector3 offset)
        {
            mergedFoodList.Add(item);
            item.transform.SetParent(itemMergeRoot);
            item.transform.localPosition = offset;
        }

        public void RemoveItem(Food item,Transform parent = null)
        {
            mergedFoodList.Remove(item);
        }

        void Start()
        {
            dishRenderer = GetComponentInChildren<Renderer>();
            slider = GetComponentInChildren<Slider>(true);
            slider.maxValue = 90f;
        }

        // Update is called once per frame
        void Update()
        {
            if (slider != null)
            {
                slider.value = progressValue;
            }
        }

        public void ShowUI()
        {
            slider.gameObject.SetActive(true);
        }

        public void HideUI()
        {
            slider.gameObject.SetActive(false);
        }

        public void GetClean()
        {
            dishRenderer.material.color = Color.white;
            HideUI();
            dirty = false;

        }

        public void GetDirty()
        {
            dishRenderer.material.color = Color.black;
            dirty = true;
        }
    }
}

