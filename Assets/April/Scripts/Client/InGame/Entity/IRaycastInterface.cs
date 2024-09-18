using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public interface IRaycastInterface
    {
        void Interact(CharacterBase character);
        void Exit();
    }
}