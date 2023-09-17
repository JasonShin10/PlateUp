using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameLifeSystem : MonoBehaviour
{
    public static IngameLifeSystem Instance { get; private set; }

    public int life = 3;
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }


}
