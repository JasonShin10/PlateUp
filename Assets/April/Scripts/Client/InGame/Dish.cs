using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : InteractionItem
{
    public Renderer dishRenderer;
    public bool dirty;
    // Start is called before the first frame update
    void Start()
    {
        dishRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDirty()
    {
        dishRenderer.material.color = Color.black;
        dirty = true;
    }
    
}
