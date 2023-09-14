using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.Events;

namespace April
{
    public enum CharacterType
    {
        None = 0,
        Customer,
        Waitress,
        Player,
    }

    public abstract class CharacterBase : MonoBehaviour, IRaycastInterface
    {
        void OnDrawGizmos()
        {
            if (NavAgent)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position + Vector3.up, NavAgent.destination + Vector3.up);
            }
        }

        public abstract CharacterType CharacterType { get; }

        public virtual bool IsAutoInteractable { get; }
        [field: SerializeField] public VisualizationCharacter Visualization { get; private set; }
        [field: SerializeField] public NavMeshAgent NavAgent { get; private set; }

        public bool moving;
        protected virtual void Awake()
        {
            NavAgent.stoppingDistance = 1f;
        }

        public virtual void Interact(CharacterBase character)
        {

        }

        public virtual void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("InteractionObject");
        }

        protected virtual void OnDestroy()
        {

        }

        public InteractionItem item;
        public event Action OnDestination;

        public float distanceBetweenDestination;

        protected virtual void Update()
        {
            distanceBetweenDestination = Vector3.Distance(transform.position, NavAgent.destination);
            if (distanceBetweenDestination <= NavAgent.stoppingDistance)
            {
                moving = false;
                if (OnDestination != null)
                {                    
                    OnDestination?.Invoke();
                    OnDestination = null;
                }
            }          
        }

        public void SetDestination(Vector3 destination, Action onDestinationCallback = null)
        {
            float distance = Vector3.Distance(destination, transform.position);
            if (distance <= NavAgent.stoppingDistance)
            {
                onDestinationCallback?.Invoke();
            }
            else
            {
                moving = true;
                NavAgent.SetDestination(destination);

                if (onDestinationCallback != null)
                {
                    OnDestination += onDestinationCallback;
                }
            }
        }


        public void Exit()
        {

        }

    }
}

