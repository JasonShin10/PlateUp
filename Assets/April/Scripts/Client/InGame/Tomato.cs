
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace April
{
    public class Tomato : Ingredient, IButtonInteract
    {
        
        public MeshFilter slicedTomatoMesh;
        private MeshFilter tomatoMesh;
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

        public float speed = 10;
        // Start is called before the first frame update
        void Start()
        {
            tomatoMesh = GetComponentInChildren<MeshFilter>(true);
            slider = GetComponentInChildren<Slider>(true);
            slider.maxValue = MaxValue;
        }
        public void ShowUI()
        {
            slider.gameObject.SetActive(true);
        }

        public void HideUI()
        {
            slider.gameObject.SetActive(false);
        }
        public void ButtonInteract()
        {

            if (ProgressValue < maxValue)
            {
                ProgressValue += speed * Time.deltaTime;
            }

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
            tomatoMesh.sharedMesh = slicedTomatoMesh.sharedMesh;
            ingredientType = IngredientList.Tomato;
            sliced = true;
        }
    }
}
