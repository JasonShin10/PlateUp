using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class CustomerTable_InteractSlot : MonoBehaviour
    {
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
    }
}