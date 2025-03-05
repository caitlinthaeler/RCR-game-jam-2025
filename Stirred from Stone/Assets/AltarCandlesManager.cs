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

    public void CheckCandlesLit(int candleLit=0)
    {
        candlesLit += candleLit;
        Debug.Log("checking candles are lit");
        if (!finishedLightingCandles && candlesLit == requiredLitCandles)
        {
            Debug.Log("All candles are lit");
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
