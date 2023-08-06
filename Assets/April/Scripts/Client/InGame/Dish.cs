using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace April
{
public class Dish : InteractionItem
{
    public Renderer dishRenderer;
    public bool dirty;

    public Slider slider;
    public float progressValue;
    // Start is called before the first frame update
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
        dirty = false;

    }

    public void GetDirty()
    {
        dishRenderer.material.color = Color.black;
        dirty = true;
    }
    
}


}

