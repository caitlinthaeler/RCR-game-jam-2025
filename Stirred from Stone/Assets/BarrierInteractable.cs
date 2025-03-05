using UnityEngine;
using System;
public class BarrierInteractable : MonoBehaviour, IInteractable
{
    public string Name => "Barrier";
    public static BarrierInteractable Instance { get; private set; }
    public event Action OnEnterBarrier;
    public bool gameOver = false;


    private void Start()
    {
        
    }

    public void Interact()
    {
        //
    }

    void Update()
    {

    }

    void OnCollisionEnter()
    {
        if (!gameOver)
        {
            OnEnterBarrier.Invoke();
        }
    }
}
