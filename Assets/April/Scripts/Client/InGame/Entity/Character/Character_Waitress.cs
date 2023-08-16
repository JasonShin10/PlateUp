using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class Character_Waitress : CharacterBase
    {
        public override CharacterType CharacterType => CharacterType.Waitress;


        public Transform waitingPosition;

        private Customer currentTargetCustomer;

        [Button("Receive Customer Order")]
        public void ReceiveCustomerOrder(Customer customer)
        {
            currentTargetCustomer = customer;
            this.SetDestination(customer.transform.position, OnDestinationCustomer);
        }

        protected override void Update()
        {
            base.Update();

            // To do : Ray Casting For Customer
            // 1. if RayHit Customer ? => OnDestinationCustomer();
            // 2. Vector3.Distance(currentTargetCustomer.transform.position transform.position) <= 1f ? => OnDestinationCustomer();
            // 3. this.SetDestination(customer.transform.position, OnDestinationCustomer);
        }

        private void OnDestinationCustomer()
        {
            if (currentTargetCustomer)
            {
                currentTargetCustomer.Interact(null);
                currentTargetCustomer = null;

                this.SetDestination(waitingPosition.position);
            }
        }
    }
}

