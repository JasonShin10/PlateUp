
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class Ingredient : InteractionItem, IButtonInteract
    {
        public IngredientList? ingredientType = null;

        public bool sliced = false;

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
      

        public virtual void ButtonInteract()
        {

        }
    }

}

