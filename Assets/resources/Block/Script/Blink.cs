using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public bool BlinkStatus = false;            //이 속성으로 블링크 On 인지 Off 인지 확인/설정 가능
    public Sprite Blink_On;                     //블링크 온 일때 스프라이트
    public Sprite Blink_Off;                    //블링크 오프 일때 스프라이트
    public float BlinkRate = 1.5f;              //블링크 주기
    public float GameStartBlinkRate = 1.5f;     //게임 시작시 블링크 주기      
    float PreBlinkRate;                         //바뀌기 전의 블링크 주기값을 저장하는 변수

    void Start ()
    {
        PreBlinkRate = BlinkRate;
        ChangeBlinkStatus();
        InvokeRepeating("Blink_Block", GameStartBlinkRate, BlinkRate);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (PreBlinkRate != BlinkRate)           //블링크 주기값 바뀔시 처리부분
        {
            CancelInvoke("Blink_Block");
            PreBlinkRate = BlinkRate;
            InvokeRepeating("Blink_Block", BlinkRate, BlinkRate);
        }
    }

    void Blink_Block()        //블럭 블링크 함수
    {
        ChangeBlinkStatus();
    }

    void ChangeBlinkStatus()
    {
        if (BlinkStatus == true)     //블링크 on 이면
        {
            transform.GetComponents<BoxCollider2D>()[0].enabled = false;    //박스콜리더 비활성화
            transform.GetComponents<BoxCollider2D>()[1].enabled = false;
            transform.GetComponent<SpriteRenderer>().sprite = Blink_Off;    //스프라이트 변경
            BlinkStatus = !BlinkStatus;
        }
        else    //블링크 off 이면
        {
            transform.GetComponents<BoxCollider2D>()[0].enabled = true;    //박스콜리더 비활성화
            transform.GetComponents<BoxCollider2D>()[1].enabled = true;
            transform.GetComponent<SpriteRenderer>().sprite = Blink_On;   //스프라이트 변경
            BlinkStatus = !BlinkStatus;
        }
    }
}
