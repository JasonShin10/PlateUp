using April;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameLifeSystem : MonoBehaviour
{
    public static IngameLifeSystem Instance { get; private set; }

    public int life = 2;

    public void Start()
    {
        Customer.onLooseLife += takeLife; 
    }

    public void takeLife()
    {
        IngameUI.Instance.hearts[life].enabled = false;
        life--;
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
