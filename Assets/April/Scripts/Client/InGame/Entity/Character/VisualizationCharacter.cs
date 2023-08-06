using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class VisualizationCharacter : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }


        public const string AnimatorParameterName_MoveDelta = "MoveDelta";
        public const string AnimatorParameterName_IsInteraction_Cook = "IsInteraction_Cook";

        private int AnimatorHashKey_MoveDelta;
        private int AnimatorHashKey_IsInteraction_Cook;

        private void Awake()
        {
            AnimatorHashKey_MoveDelta = Animator.StringToHash(AnimatorParameterName_MoveDelta);
            AnimatorHashKey_IsInteraction_Cook = Animator.StringToHash(AnimatorParameterName_IsInteraction_Cook);
        }

        public void SetMovement(float movement)
        {
            Animator.SetFloat(AnimatorHashKey_MoveDelta, movement);
        }

        public void SetInteractionCook(bool isInteraction)
        {
            Animator.SetBool(AnimatorHashKey_IsInteraction_Cook, isInteraction);
        }
    }
}

