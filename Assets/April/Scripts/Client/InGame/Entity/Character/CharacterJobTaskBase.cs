using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    [Serializable]
    public class CharacterJobTaskBase
    {
        public Vector3 destination;
        public Action executeAction;
    }

    public class ReceiveOrderJob : CharacterJobTaskBase
    {
        public Customer customer;
        public Character_Waitress actor;

        public ReceiveOrderJob(Customer customer, Character_Waitress actor)
        {
            this.customer = customer;
            this.actor = actor;
            this.executeAction = OnExecuteReceiveOrder;
        }

        void OnExecuteReceiveOrder()
        {
            actor.SetDestination(customer.transform.position, OnDestinationCustomer);
        }

        void OnDestinationCustomer()
        {
            if (customer)
            {
                customer.Interact(actor);

                //if (IngameWaiterSystem.Instance.waitingOrderCustomerList.Count > 0)
                //{
                //    actor.ReceiveCustomerOrder(IngameWaiterSystem.Instance.waitingOrderCustomerList[0]);
                //}
                //else if (IngameWaiterSystem.Instance.waitingFoodCustomerList.Count > 0 && waitressTable.HasFood)
                //{
                //    actor.AddJobTask(waitressTable.InteractionPoint.position, OnWaitressArrivedTable);
                //    //this.SetDestination(waitressTable.InteractionPoint.position, OnWaitressArrivedTable);
                //}
                //else
                //{
                //    actor.SetDestination(waitingPosition.position);
                //}
            }
        }
    }
}

