using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D Col)
    {
        if(Col.transform.name == "Player_Foot" || Col.transform.name == "Player")
        {
            uint KeyCount = ++GameObject.Find("Player").GetComponent<Player>().KeyCount;
            GameObject.Find("Canvas").transform.Find("KeyCount").GetComponent<Text>().text = "열쇠:" + KeyCount;
            Destroy(transform.gameObject);
        }
    }
}
