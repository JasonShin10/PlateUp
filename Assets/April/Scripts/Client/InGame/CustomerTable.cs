using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class CustomerTable : InteractionBase
    {
        public override bool IsAutoInteractable => false;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.CustomerTable;

        private PlayerController player;
        private InteractionItem item;


        public bool customerVisted;
        [SerializeField] public List<Chair> chiars = new List<Chair>();
        public List<InteractionItem> dishes;
        public InteractionItem nearestDish;
        void CustomerTableInteract()
        {
            float nearestDistance = float.MaxValue;
            if (player.item == null)
            {
                foreach(Dish dish in dishes)
                {
                    float distance = Vector3.Distance(player.transform.position, dish.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestDish = dish;
                    }
                }
                player.item = nearestDish;
                player.item.transform.SetParent(player.transform);
                player.item.transform.localPosition = Vector3.up + Vector3.forward;
                nearestDish = null;
            }
        }

        public override void Interact(PlayerController player)
        {
            this.player = player;
            CustomerTableInteract();
        }

        public override void Exit()
        {

        }
    }
}

