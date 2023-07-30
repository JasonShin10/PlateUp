using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class VisualizationCharacter : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }




        public const string AnimatorParameterName_MoveDelta = "MoveDelta";

        private int AnimatorHashKey_MoveDelta;

        private void Awake()
        {
            AnimatorHashKey_MoveDelta = Animator.StringToHash(AnimatorParameterName_MoveDelta);
        }

        public void SetMovement(float movement)
        {
            Animator.SetFloat(AnimatorHashKey_MoveDelta, movement);
        }
    }
}

