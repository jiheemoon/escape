using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectPoint : MonoBehaviour
{
    public float FallingRate = 1f;
    public float GameStartFallingRate = 1.5f;      //게임 시작시 발사 주기

    void Start ()
    {
        InvokeRepeating("Summon", GameStartFallingRate, FallingRate);
        transform.GetComponent<SpriteRenderer>().sprite = null;
    }
	
    void Summon()
    {
        Instantiate(Resources.Load("Block/Object/FallingObject"), transform.position,Quaternion.identity);
    }
}
