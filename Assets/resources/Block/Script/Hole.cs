using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public GameObject TeleportTo;

    void OnCollisionEnter2D(Collision2D Col)
    {
        if(Col.transform.name == "Player_Foot" || Col.transform.name == "Player")
        {
            GameObject.Find("Player").transform.position = TeleportTo.transform.position;
        }
    }

}
