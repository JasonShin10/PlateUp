using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool isVisited = false;
    public bool istargeted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Customer>().target == this)
        {
            isVisited = true;
        }
    }
}
