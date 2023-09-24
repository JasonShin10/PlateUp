using April;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameLifeSystem : MonoBehaviour
{
    public static IngameLifeSystem Instance { get; private set; }

    public int life = 2;

    public CinemachineVirtualCameraBase camera1;
    public CinemachineVirtualCameraBase camera2;
    public void Start()
    {
        Customer.onLooseLife += takeLife; 
    }

    public void takeLife()
    {
        IngameUI.Instance.hearts[life].enabled = false;
        if (life <= 0)
        {
            UIManager.Show<GameOverUI>(UIList.GameOverUI);
            camera2.gameObject.SetActive(true);
            camera1.gameObject.SetActive(false);
        }
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
