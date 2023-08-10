using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldUI : UIBase
{
    public TextMeshProUGUI nameTag;
    public Slider BeefProgress;
    public Transform characterTransform; 
    public Vector3 offset = new Vector3(0, 2, 0);
    public Camera mainCamera;

    private void Start()
    {
        TextMeshProUGUI nameTagPrefab = Instantiate(nameTag, this.transform.position, Quaternion.identity, transform);
        nameTagPrefab.text = "Jack";
    }
    private void Update()
    {
        transform.position = characterTransform.position + offset;
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }

    //private void OnEnable()
    //{
    //    Stove.OnBeefCreated += ShowBeefUI;
    //}

    //private void OnDisable()
    //{
    //    Stove.OnBeefCreated -= ShowBeefUI;
    //}

    // ThickBeef할때 오버로딩 해야할까?
    public void ShowBeefUI(Beef Beef)
    {
        Debug.Log("111");

        Slider sliderPrefab = Instantiate(BeefProgress,transform.position, Quaternion.identity, transform);
        sliderPrefab.maxValue = 90;
        sliderPrefab.value = Beef.progressValue; 
        Beef.slider = sliderPrefab;
    }
}
