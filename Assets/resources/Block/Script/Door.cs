using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D Col)
    {
        if(Col.transform.name == "Player")
        {
            StartCoroutine(GameObject.Find("GameManager").GetComponent<GameManager>().GameClear());
        }
    }
}
