using UnityEngine;
using System;

public class StoneManager : MonoBehaviour
{
    public static event Action OnStonesSealed;

    private int stonesCollected = 0;
    public int totalStonesRequired;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnStone()
    {
        stonesCollected++;
        if (stonesCollected == totalStonesRequired)
        {
            Debug.Log("All stones have been returned!");
        }
    }

    public void PourHolyWater()
    {
        // Seal the stone with holy water
        if (stonesCollected == totalStonesRequired)
        {
            OnStonesSealed?.Invoke();
            Debug.Log("The holy water has sealed all of the stones in place!");
        }
    }

    
}
