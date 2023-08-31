using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


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
        public abstract CharacterType CharacterType { get; }

        public virtual bool IsAutoInteractable { get; }
        [field: SerializeField] public VisualizationCharacter Visualization { get; private set; }
        [field: SerializeField] public NavMeshAgent NavAgent { get; private set; }
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

        private float distanceBetweenDestination;

        protected virtual void Update()
        {
            distanceBetweenDestination = Vector3.Distance(transform.position, NavAgent.destination);
            if (distanceBetweenDestination <= NavAgent.stoppingDistance)
            {
                Debug.Log("Arrived at the destination. Invoking the callback...");
                if (OnDestination != null)
                {
                    Debug.Log($"OnDestination is assigned to: {OnDestination.Method.Name}");
                }
                else
                {
                    Debug.Log("OnDestination is null");
                }
                OnDestination?.Invoke();
                // 이부분이 문제인거 같다..
                OnDestination = null;
            }
        }

        public void SetDestination(Vector3 destination, Action onDestinationCallback = null)
        {
            NavAgent.SetDestination(destination);
            Debug.Log($"Incoming callback: {onDestinationCallback}");
            if (onDestinationCallback != null)
            {
                OnDestination = onDestinationCallback;
                Debug.Log($"OnDestination assigned: {OnDestination}");
            }
        }

       public void Exit()
        {

        }

    }
}

