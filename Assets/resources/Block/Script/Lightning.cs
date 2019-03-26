using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D Col)
    {
        if(Col.transform.name == "Player")
        {
            GameObject.Find("Player").GetComponent<Player>().Helth = 0;
        }
    }
}
