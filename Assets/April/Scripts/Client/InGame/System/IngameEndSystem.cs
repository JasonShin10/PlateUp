using April;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameEndSystem : MonoBehaviour
{
    public static IngameEndSystem Instance { get; private set; }
    public bool End { get; private set; }

    public event Action OnGameCleared;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    public void HandleGameOver()
    {
        End = true;
        OnGameCleared?.Invoke();
    }
}
