using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerFactory : MonoBehaviour
{
    public GameObject customer;

    float currentTime;
    [SerializeField] private float spawnTime = 3f;
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > spawnTime)
        {
        GameObject customerInstance = Instantiate(customer,transform.position, Quaternion.identity);
            currentTime =0;

        }
    }
}
