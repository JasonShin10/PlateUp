using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleObject : MonoBehaviour
{
    public SampleData sampleData;

    private void Update()
    {
        transform.position += Vector3.forward * sampleData.MoveSpeed * Time.deltaTime;
    }
}
