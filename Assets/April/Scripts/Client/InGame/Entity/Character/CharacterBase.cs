using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using static April.Customer;

namespace April
{
    public enum CharacterType
    {
        None = 0,
        Customer,
        Waitress,
    }

    public abstract class CharacterBase : MonoBehaviour
    {
        public abstract CharacterType CharacterType { get; }

        public virtual bool IsAutoInteractable { get; }
        [field: SerializeField] public VisualizationCharacter Visualization { get; private set; }
        [field: SerializeField] public NavMeshAgent NavAgent { get; private set; }
        protected virtual void Awake()
        {            
            NavAgent.stoppingDistance = 1f;
        }
        public virtual void Interact(PlayerController player)
        {

        }


        protected virtual void OnDestroy()
        {

        }

        public event Action OnDestination;

        private float distanceBetweenDestination;

        protected virtual void Update()
        {
            distanceBetweenDestination = Vector3.Distance(transform.position, NavAgent.destination);
            if (distanceBetweenDestination <= NavAgent.stoppingDistance)
            {
                OnDestination?.Invoke();
                OnDestination = null;
            }
        }

        public void SetDestination(Vector3 destination, Action onDestinationCallback = null)
        {
            NavAgent.SetDestination(destination);
            if (onDestinationCallback != null)
            {
                OnDestination = onDestinationCallback;
            }
        }
    }
}

