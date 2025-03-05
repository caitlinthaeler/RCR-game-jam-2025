using UnityEngine;
using System;

public class AltarCandlesManager : MonoBehaviour
{
    public static AltarCandlesManager Instance { get; private set; }
    public event Action OnAllCandlesLit;
    public GameObject stone;
    private int candlesLit;
    private bool finishedLightingCandles = false;
    public int requiredLitCandles = 4;
    public AudioSource audioSource;
    public AudioClip stoneRisingSound;
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
    void Start()
    {
        candlesLit = 0;
        stone.GetComponent<BoxCollider>().enabled = false;
    }

    public void MinusCandleslit()
    {
        candlesLit -= 1;
    }

    public void AddCandleslit()
    {
        candlesLit += 1;
    }

    public void CheckCandlesLit()
    {
        if (!finishedLightingCandles && candlesLit == requiredLitCandles)
        {
            OnAllCandlesLit.Invoke();
            if (audioSource && stoneRisingSound)
            {
                audioSource.PlayOneShot(stoneRisingSound);
            }
            stone.GetComponent<MovementAnimator>().StartMovement(OnRevealComplete);
        }
    }

    public void OnRevealComplete()
    {
        stone.GetComponent<BoxCollider>().enabled = true;
    }
}
