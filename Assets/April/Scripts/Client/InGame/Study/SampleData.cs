using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SampleData", menuName = "April/Data/SampleData")]
public class SampleData : ScriptableObject
{
    public float MoveSpeed;

    public int SampleValue_A;
    public int SampleValue_B;
    public int SampleValue_C;
    public int SampleValue_D;
}
