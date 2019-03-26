using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Locked : MonoBehaviour
{
    bool Lock = true;
    void OnCollisionEnter2D(Collision2D Col)
    {
        if (Col.transform.name == "Player_Foot" || Col.transform.name == "Player")
        {
            if (Lock)
            {
                uint KeyCount = GameObject.Find("Player").GetComponent<Player>().KeyCount;
                if (KeyCount > 0)
                {
                    KeyCount = --GameObject.Find("Player").GetComponent<Player>().KeyCount;
                    GameObject.Find("Canvas").transform.Find("KeyCount").GetComponent<Text>().text = "열쇠:" + KeyCount;
                    Lock = false;
                    Destroy(transform.gameObject);
                }
            }
        }
    }
}
