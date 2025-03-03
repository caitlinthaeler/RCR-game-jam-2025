using UnityEngine;
using System;
using System.Collections.Generic;

public class BellManager : MonoBehaviour
{
    public static BellManager Instance { get; private set; }
    public event Action OnBellsRungInCorrectOrder;
    public List<int> bellOrder;
    private int bellIndex;

    private void Awake()
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

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bellOrder = new List<int>();
        bellIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RingBell(int bellNumber)
    {
        return;
    //     DialogueManager.Instance.AddActionDialogue($"You rang bell {bellNumber}.");
    //     if (bellNumber == bellOrder[bellIndex])
    //     {
    //         bellIndex++;
    //         if (bellIndex == bellOrder.Count)
    //         {
    //             OnBellsRungInCorrectOrder?.Invoke();
    //         }
    //     }
    //     else
    //     {
    //         bellIndex = 0;
    //     }
    }
}
