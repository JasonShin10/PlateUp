
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace April
{
    public class Cabbage : Ingredient, IButtonInteract
    {

        public GameObject slicedCabbage;
        public GameObject cabbage;
        public Slider slider;


        [SerializeField] private float maxValue;
        public float ProgressValue { get; set; }
        public float MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
            }
        }
        
        public float speed = 10f;
        // Start is called before the first frame update
        void Start()
        {

  
            slider = GetComponentInChildren<Slider>(true);
            slider.maxValue = MaxValue;
        }
        public void ShowUI()
        {
            slider.gameObject.SetActive(true);
        }

        public void ButtonInteract()
        {
            if (ProgressValue < maxValue)
            {
            ProgressValue += speed * Time.deltaTime;
            }
        }

        public void HideUI()
        {
            slider.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (slider != null)
            {
                slider.value = ProgressValue;
            }
            if (slider.value == slider.maxValue)
            {
                MeshChange();
                HideUI();
            }
        }

        private void MeshChange()
        {

            ingredientType = IngredientList.Cabbage;
            cabbage.SetActive(false);
            slicedCabbage.SetActive(true);
            sliced = true;
        }
    }
}
