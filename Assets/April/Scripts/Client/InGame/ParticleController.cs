using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class ParticleController : MonoBehaviour
    {
        public ParticleSystem myParticleSystem;
        private void Start()
        {
            myParticleSystem.Stop();
        }
        public void PlayParticle()
        {
            myParticleSystem.Play();
        }

        public void StopParticle()
        {

            //myParticleSystem.Stop();
        }
    }
}
