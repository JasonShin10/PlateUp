
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace April
{
    public class Dish : InteractionItem
    {
        //public List<Food> ContainedFoodItems => mergedFoodList;

        public List<Food> ContainedFoodItems
        {
                get { return mergedFoodList; }
        }
        public Renderer dishRenderer;
        public bool dirty;
        public Slider slider;
        public float progressValue;

        public Transform itemMergeRoot;
        public List<Food> mergedFoodList = new List<Food>();
        public Salad salad;


        public List<IngredientList> ingredients = new List<IngredientList>
        {IngredientList.Cabbage,
        IngredientList.Tomato
        };
        public float offset = 0.1f;
        public Transform spawnPoint;
        public void AddItem(Food item, Vector3 offset)
        {
            mergedFoodList.Add(item);
            item.transform.SetParent(itemMergeRoot);
            item.transform.position = offset;
        }

        public void AddItem(Ingredient item, Vector3 offset)
        {
            if (item.ingredientType.HasValue)
            {
                IngredientList ingredient = item.ingredientType.Value;
                item.transform.SetParent(itemMergeRoot);
                item.transform.position = offset;
                if (ingredients.Contains(ingredient))
                {
                    ingredients.Remove(ingredient);
                }
            }
            if (CanCreateSalad())
            {
                Salad saladItem = Instantiate(salad);
                foreach (Transform child in itemMergeRoot)
                {
                    Destroy(child.gameObject);
                }
                AddItem(saladItem, offset);
            }
        }

        public bool CanCreateSalad()
        {
            if (ingredients.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveItem(Food item, Transform parent = null)
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

