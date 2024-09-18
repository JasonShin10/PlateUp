using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameDaySystem : MonoBehaviour
{
    public static IngameDaySystem Instance { get; private set; }
    public int Day { get; private set; }
    private int day =0;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log(this.name);
      
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    public void SetDay()
    {
        Day = ++day;

        IngameUI.Instance.SetDayText(Day);
    }
}
