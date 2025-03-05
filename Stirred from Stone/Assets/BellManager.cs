using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class BellSelection
{
    [Range(1, 8)]
    public int nextBell;
}

public class BellManager : MonoBehaviour
{
    public static BellManager Instance { get; private set; }
    public event Action OnBellsRungInCorrectOrder;
    public List<BellSelection> bellOrder;
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
        bellIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RingBell(int bellNumber)
    {
        DialogueManager.Instance.AddActionDialogue($"You rang bell {bellNumber}.");
        if (bellNumber == bellOrder[bellIndex].nextBell)
        {
            bellIndex++;
            if (bellIndex == bellOrder.Count)
            {
                OnBellsRungInCorrectOrder?.Invoke();
            }
        }
        else
        {
            bellIndex = 0;
        }
    }
}
