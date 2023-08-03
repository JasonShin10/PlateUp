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
        Customer customer = other.transform.GetComponent<Customer>();
        if (customer != null && customer.target == this)
        {
            isVisited = true;
        }
    }
}
