using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace April
{
    public class BillboardCanvasUI : MonoBehaviour
    {
        private void LateUpdate()
        {
            Refresh();
        }

        public void Refresh()
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}
