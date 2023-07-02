using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public abstract class SceneBase : MonoBehaviour
    {
        public abstract bool IsAdditiveScene { get; }
        public abstract IEnumerator OnStart();
        public abstract IEnumerator OnEnd();
    }
}

