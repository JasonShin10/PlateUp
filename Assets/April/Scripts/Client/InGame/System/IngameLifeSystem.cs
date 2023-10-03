using April;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameLifeSystem : MonoBehaviour
{
    public static IngameLifeSystem Instance { get; private set; }

    public bool HasRemainLife => life > 0;
    public int RemainLife => life;

    [SerializeField] private int life = 2;
    public event Action<int> OnLifeCountChanged;

    private void Start()
    {
        
        Customer.OnLooseLife += TakeLife;
    }

    public void TakeLife()
    {
        if (IngameEndSystem.Instance.End == true)
        {
            return;
        }
        life--;
        OnLifeCountChanged?.Invoke(life);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }


}
