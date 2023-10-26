
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
      
        
        public float speed = 10f;

        void Start()
        {

            slider.maxValue = MaxValue;
        }
        public override void ShowUI()
        {
            slider.gameObject.SetActive(true);
        }

        public override void ButtonInteract()
        {
            if (ProgressValue < maxValue)
            {
            ProgressValue += speed * Time.deltaTime;
            }
        }

        public override void  HideUI()
        {
            slider.gameObject.SetActive(false);
        }


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
