using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class CustomerTable_InteractSlot : MonoBehaviour
    {
        //public override bool IsAutoInteractable => false;
        //public override InteractionObjectType InterationObjectType => InteractionObjectType.CustomerTable;

        //public CustomerTable parentTable;

        public bool CustomerAssigned { get; private set; }
        public Customer AssignedCustomer => assignedCustomer;


        private Customer assignedCustomer;

        public void SetAssigned(Customer owner)
        {
            assignedCustomer = owner;
            CustomerAssigned = true;
        }

        public void Unssigned()
        {
            assignedCustomer = null;
            CustomerAssigned = false;
        }

        //public InteractionItem item;


        //public override void Exit()
        //{

        //}

        //public void GroupCheck()
        //{

        //}

        //public override void Interact(PlayerController player)
        //{
        //    if (item == null)
        //        return;

        //    player.item = item;
        //    player.item.transform.SetParent(player.transform);
        //    player.item.transform.localPosition = Vector3.up + Vector3.forward;
        //}
    }
}