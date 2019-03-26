using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bollet : MonoBehaviour
{
    public int GunBlockID = 0;
    public Vector2 Direction;
    public float Speed;

	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Direction * Speed * Time.deltaTime);
	}

    void OnTriggerEnter2D(Collider2D Col)
    {
        if (Col.gameObject.GetInstanceID() != GunBlockID)       //충돌한 물체가 총 블록 이 아닐경우 
        {
            if (Col.name == "Player")
            {
                GameObject.Find("Player").GetComponent<Player>().Helth = 0;
            }

            if(Col.tag == "Block" || Col.name == "Player" || Col.tag == "Wall") Destroy(transform.gameObject);

        }
    }
}
