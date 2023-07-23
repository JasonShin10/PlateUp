using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace April
{
    public class Customer : MonoBehaviour
    {
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
            orderImageDisplay.gameObject.SetActive(true);
        }
    }
}

