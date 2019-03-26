using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Helth = 5;                    //플레이어 채력
    public float Speed = 2.5f;               //플레이어 이동속도
    public float JumpPower = 250f;          //플레이어 점프력
    public float AvailableJumpCount = 0;     //플레이어가 공중에서 점프 가능한 횟수
    public bool Jumping = false;             //플레이어가 점프하고 있는지 체크
    public uint KeyCount = 0;               //플레이어가 가지고 있는 열쇠갯수 

    // Update is called once per frame
    void Update ()
    {
        StartCoroutine(PlayerMove());
    }

    IEnumerator PlayerMove()
    {
        if (Helth != 0 && !GameObject.Find("GameManager").GetComponent<GameManager>().GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) && AvailableJumpCount > 0)   //유저가 스패이스 키를 눌렀고 점프 가능횟수가 0 이상이면
            {
                transform.GetComponent<AudioSource>().Play();
                transform.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.GetComponent<Rigidbody2D>().velocity.x, 0);   //플레이어의 y 축 가속도를 0 으로 만들고
                transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpPower);     //플레이어를 점프시킨다.
                Jumping = true;
                AvailableJumpCount--;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector2.left * Speed * Time.deltaTime);     //왼쪽 화살표키 누르면 왼쪽으로 이동
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector2.right * Speed * Time.deltaTime);    //오른쪽 화살표키 누르면 오른쪽으로 이동
            }
        }
        yield return null;
    }
}
