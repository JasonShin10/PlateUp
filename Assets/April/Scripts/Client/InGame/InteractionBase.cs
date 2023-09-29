using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public enum InteractionObjectType
    {
        None = 0,

        Stove,
        Container,
        Table,
        DishTable,
        CustomerTable,
        DishWasher,
        TrashCan,
        CabbageContainer,
        WaitressTable
    }

    public abstract class InteractionBase : MonoBehaviour,IRaycastInterface
    {
        public static Dictionary<InteractionObjectType, List<InteractionBase>> SpawnedInteractionObjects
            = new Dictionary<InteractionObjectType, List<InteractionBase>>();

        //public RuntimeCollection_InteractionObjects runtimeCollection;
        public MeshRenderer meshRenderer;
        public Material outLineMaterial;
        private string materialName = "OutLineMaterial";
        public abstract bool IsAutoInteractable { get; }
        public abstract InteractionObjectType InterationObjectType { get; }
        public abstract void Interact(CharacterBase character);
        public abstract void Exit();

        protected virtual void Awake()
        {
            RegistObject();
        }

        protected virtual void OnDestroy()
        {
            RemoveRegist();
        }

        public virtual void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("InteractionObject");

        }
        public void PutOutLineMaterial()
        {
            meshRenderer.enabled = true;
        }
        private void Update()
        {
            
        }
        public void TakeOutOutLineMaterial()
        {
            meshRenderer.enabled = false;
        }
        void RegistObject()
        {

            // 2. Static 변수로 제어하는 방식
            if (SpawnedInteractionObjects.TryGetValue(InterationObjectType, out List<InteractionBase> container))
            {
                container.Add(this);
            }
            else
            {
                SpawnedInteractionObjects.Add(InterationObjectType, new List<InteractionBase>() { this });
            }
        }

        void RemoveRegist()
        {
            if (SpawnedInteractionObjects.TryGetValue(InterationObjectType, out List<InteractionBase> container))
            {
                container.Remove(this);
            }
        }
    }


}
            // 1. Runtime Collection ( Scriptable Object에 컨테이너를 선언해서 사용하는 방식)
            //if (runtimeCollection.SpawnedObjectContainer.TryGetValue(InterationObjectType, out List<InteractionBase> container2))
            //{
            //    container2.Add(this);
            //}
            //else
            //{
            //    runtimeCollection.SpawnedObjectContainer.Add(InterationObjectType, new List<InteractionBase>() { this });
            //}

