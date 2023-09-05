using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace April
{
    public abstract class Food : InteractionItem
    {
        public abstract MenuList MenuType { get; }
        public virtual int CookingState { get; }
        public float progressValue;

        public float offsetOnDish;

        public virtual void ShowUI()
        {

        }
        public virtual void HideUI()
        {

        }
    }
}
