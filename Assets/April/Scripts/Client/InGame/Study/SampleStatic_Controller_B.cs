using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleStatic_Controller_B : MonoBehaviour
{
    private void Awake()
    {
        StaticSample.StaticSampleValue = 20;        

        int a = 100; // Stack - 지역 변수
        DoAction(10);
    }

    void DoAction(int actionIndex)
    {
        // Parameter : actionIndex  - Stack -> 지역변수같은 느낌
    }
}
