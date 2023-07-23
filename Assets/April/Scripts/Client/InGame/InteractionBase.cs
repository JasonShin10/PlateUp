using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public abstract class InteractionBase : MonoBehaviour
    {
        public abstract bool IsAutoInteractable { get; }
        public abstract void Interact(PlayerController player);
        public abstract void Exit();
    }


}

