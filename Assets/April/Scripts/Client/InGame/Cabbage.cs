using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace April
{
    public class Cabbage : InteractionItem, IButtonInteract
    {
        public MeshFilter slicedCabbageMesh;
        private MeshFilter cabbageMesh;
        public Slider slider;
        public float progressValue;
        public bool onTable;
        public float speed = 10f;
        // Start is called before the first frame update
        void Start()
        {
            cabbageMesh = GetComponentInChildren<MeshFilter>(true);
            slider = GetComponentInChildren<Slider>(true);
            slider.maxValue = 90f;
        }
        public void ShowUI()
        {
            slider.gameObject.SetActive(true);
        }

        public void ButtonInteract()
        {
            progressValue += speed * Time.deltaTime;
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
                slider.value = progressValue;
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
        }
    }
}
