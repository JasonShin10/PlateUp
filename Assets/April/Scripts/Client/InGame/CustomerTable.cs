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
        private Food foodComponent;

        public bool customerVisted;
        [SerializeField] public List<Chair> chiars = new List<Chair>();

        void CustomerTableInteract()
        {
            if (player.item != null)
            {
                foodComponent = player.item;
                
                foodComponent.transform.SetParent(this.transform);
                foodComponent.transform.localPosition = Vector3.up;
                foodComponent.gameObject.SetActive(true);
                player.item = null;
                Debug.Log("Item Insert To CustomerTable!");
            }
            else if (player.item == null)
            {
                player.item = foodComponent;
               
                player.item.transform.SetParent(player.transform);
                player.item.transform.localPosition = Vector3.up + Vector3.forward;
                foodComponent = null;
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

