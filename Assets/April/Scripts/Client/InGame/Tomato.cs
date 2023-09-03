using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace April
{
    public class Tomato : InteractionItem, IButtonInteract
    {
        public MeshFilter slicedTomatoMesh;
        private MeshFilter tomatoMesh;
        public Slider slider;
        public float progressValue;
        public bool onTable;
        public float speed = 10;
        // Start is called before the first frame update
        void Start()
        {
            tomatoMesh = GetComponentInChildren<MeshFilter>(true);
            slider = GetComponentInChildren<Slider>(true);
            slider.maxValue = 90f;
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
            progressValue += speed * Time.deltaTime;
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
            tomatoMesh.sharedMesh = slicedTomatoMesh.sharedMesh;
        }
    }
}
