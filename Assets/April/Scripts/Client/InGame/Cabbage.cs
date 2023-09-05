
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace April
{
    public class Cabbage : Ingredient, IButtonInteract
    {
        
        public MeshFilter slicedCabbageMesh;
        private MeshFilter cabbageMesh;
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
            cabbageMesh = GetComponentInChildren<MeshFilter>(true);
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
            cabbageMesh.sharedMesh = slicedCabbageMesh.sharedMesh;
            ingredientType = IngredientList.Cabbage;
            sliced = true;
        }
    }
}
