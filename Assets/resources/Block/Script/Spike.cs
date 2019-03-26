using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D Col)
    {
        if(Col.transform.name == "Player")
        {
            GameObject.Find("Player").GetComponent<Player>().Helth = 0;
        }
    }
}
