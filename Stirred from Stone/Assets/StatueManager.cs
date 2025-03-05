using UnityEngine;
using System;

public class StatueManager : MonoBehaviour
{
    public static StatueManager Instance { get; private set; }
    public event Action OnBookReturned;
    public GameObject stone;
    public GameObject openBookObj;
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
        stone.GetComponent<BoxCollider>().enabled = false;
        openBookObj.SetActive(false);
    }

    public void RevealStone()
    {
        openBookObj.SetActive(true);
        stone.GetComponent<MovementAnimator>().StartMovement(OnRevealComplete);
        // else 
        // {
        //     Vector3 stonePos = stone.transform.position;
        //     stone.transform.position = new Vector3(stonePos.x, stonePos.y, stonePos.z+0.07f);
        //     stone.GetComponent<BoxCollider>().enabled = true;
        // }
    }

    public void OnRevealComplete()
    {
        Debug.Log("animation complete");
        stone.GetComponent<BoxCollider>().enabled = true;
    }
}
