using UnityEngine;
using System;

public class StatueManager : MonoBehaviour
{
    public static StatueManager Instance { get; private set; }
    public event Action OnBookReturned;
    public GameObject stone;
    public GameObject openBookObj;
    public Animator stoneAnimator;
    void Start()
    {
        openBookObj.SetActive(false);
    }

    public void RevealStone()
    {
        openBookObj.SetActive(true);
        if (stoneAnimator)
        {
            stoneAnimator.Play("StatueRevealStone");
        }
        else 
        {
            Vector3 stonePos = stone.transform.position;
            stone.transform.position = new Vector3(stonePos.x, stonePos.y, stonePos.z+0.07f);
            stone.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
