using UnityEngine;
using System;

public class BellTowerDoorManager : MonoBehaviour
{
    public static BellTowerDoorManager Instance { get; private set; }
    public event Action OnBellTowerDoorUnlocked;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DoorUnlocked()
    {
        OnBellTowerDoorUnlocked?.Invoke();
    }
}
