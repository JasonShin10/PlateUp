using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace April
{
    public class Customer : InteractionBase
    {
        public override bool IsAutoInteractable => true;
        public override InteractionObjectType InterationObjectType => InteractionObjectType.None;

        public List<CustomerTable> tables = new List<CustomerTable>();
        public Chair target;

        public bool last;
        public Color GraphicColor
        {
            get => graphicRenderer.material.color;
            set => graphicRenderer.material.color = value;
        }

        private NavMeshAgent agent;
        public Transform targetPosition;
        public Image orderImageDisplay;

        public Renderer graphicRenderer;

        protected override void Awake()
        {
            base.Awake();
            graphicRenderer = GetComponent<Renderer>();
        }

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            orderImageDisplay = GetComponentInChildren<Image>(true);

            if (InteractionBase.SpawnedInteractionObjects.TryGetValue(InteractionObjectType.CustomerTable, out List<InteractionBase> tables))
            {
                for (int i = 0; i < tables.Count; i++)
                {
                    var customerTable = tables[i] as CustomerTable;

                }

                foreach (InteractionBase tableEntity in tables)
                {
                    var customerTable = tableEntity as CustomerTable;

                }
            }

            //foreach (CustomerTable table in tables)
            //{
            //    if (table.customerVisted == true)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        foreach (Chair chair in table.chiars)
            //        {
            //            if (chair.isVisited != true && chair.istargeted != true)
            //            {
            //                targetPositoin = chair.transform;
            //                target = chair;
            //                target.istargeted = true;
            //                break;    
            //            }
            //        }
            //    }
            //}

            if (targetPosition != null)
            {
                MoveToTarget();
            }
        }

        // Update is called once per frame
        void MoveToTarget()
        {
            agent.SetDestination(targetPosition.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target.transform == other.transform)
            {
                orderImageDisplay.gameObject.SetActive(true);
            }
        }



        public override void Interact(PlayerController player)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}

