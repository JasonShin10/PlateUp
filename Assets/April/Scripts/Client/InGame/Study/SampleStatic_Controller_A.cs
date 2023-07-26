using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StaticSample
{
    public int SampleValue;
    public static int StaticSampleValue;

    public void DoSample() { }
    public static void DoStaticSample() { }
}

public class SampleStatic_Controller_A : MonoBehaviour
{
    private void Awake()
    {
        StaticSample sample = new StaticSample();
        sample.DoSample();
        sample.SampleValue = 10;

        StaticSample.StaticSampleValue = 10;
        StaticSample.DoStaticSample();
    }
}
