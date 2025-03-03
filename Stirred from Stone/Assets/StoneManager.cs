using UnityEngine;
using System;

public class StoneManager : MonoBehaviour
{
    public static StoneManager Instance { get; private set; }
    public event Action OnStonesSealed;

    private int stonesReturned = 0;
    public int totalStonesRequired;
    public bool allStonesReturned;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allStonesReturned = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnStone()
    {
        stonesReturned++;
        if (stonesReturned == totalStonesRequired)
        {
            allStonesReturned = true;
            OnStonesSealed?.Invoke();
            Debug.Log("All stones have been returned!");
        } else {
            Debug.Log($"{totalStonesRequired - stonesReturned} stones left to return!");
        }
    }

    public bool AllStonesReturned()
    {
        return allStonesReturned;
    }

    public int GetStonesReturned()
    {
        return stonesReturned;
    }

    public void PourHolyWater()
    {
        // Seal the stone with holy water
        if (stonesReturned == totalStonesRequired)
        {
            OnStonesSealed?.Invoke();
            Debug.Log("The holy water has sealed all of the stones in place!");
        }
    }

    
}
