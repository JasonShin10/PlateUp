using April;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Food : InteractionItem
{
    public abstract int CookingState { get; }
    public float progressValue;

    public float offsetOnDish;

    public virtual void ShowUI()
    {

    }
    public virtual void HideUI()
    {

    }
}
