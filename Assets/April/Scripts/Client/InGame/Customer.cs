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
        public List<CustomerTable> tables = new List<CustomerTable>();
        public Chair target;

        public bool last;
        public Color GraphicColor
        {
            get => graphicRenderer.material.color;
            set => graphicRenderer.material.color = value;
        }

        private NavMeshAgent agent;
        public Transform targetPositoin;
        public Image orderImageDisplay;

        public Renderer graphicRenderer;

        private void Awake()
        {
            graphicRenderer = GetComponent<Renderer>();
        }

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            orderImageDisplay = GetComponentInChildren<Image>(true);

            foreach (CustomerTable table in tables)
            {
                if (table.customerVisted == true)
                {
                    continue;
                }
                else
                {
                    foreach (Chair chair in table.chiars)
                    {
                        if (chair.isVisited != true && chair.istargeted != true)
                        {
                            targetPositoin = chair.transform;
                            target = chair;
                            target.istargeted = true;
                            break;    
                        }
                    }
                }
            }
            if (targetPositoin != null)
            {
                MoveToTarget();
            }
        }

        // Update is called once per frame
        void MoveToTarget()
        {
            agent.SetDestination(targetPositoin.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (target.transform == other.transform)
            {
            orderImageDisplay.gameObject.SetActive(true);
            }
        }

        public override bool IsAutoInteractable => true;

        public override void Interact(PlayerController player)
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}

