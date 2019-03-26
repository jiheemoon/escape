using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Bollet;                    //게임오브젝트 총알 선언
    public bool Enable = true;                   //ture 로 하면 총알 발사함
    public float FireRate = 1.5f;               //총알 발사 주기
    public float GameStartFireRate = 1.5f;      //게임 시작시 발사 주기
    float PreFireRate;                           //발사주기 변경 감지 변수
    public float BolletSpeed = 3f;               //총알 속도
    public char BolletDirection = 'U';    //'U' = UP  'R' = RIGHT  'D' = DOWN  'L' = LEFT

    // Use this for initialization
    void Start()
    {
        PreFireRate = FireRate;                 //게임을 시작할때 PreFireRate = FireRate 동기화시킴
        InvokeRepeating("Fire", GameStartFireRate, FireRate);   //InvokeRepeating 실행
    }

    // Update is called once per frame
    void Update()
    {
        if (PreFireRate != FireRate)        //발사주기의 값이 변경되면
        {
            CancelInvoke("Fire");           
            PreFireRate = FireRate;
            InvokeRepeating("Fire", FireRate, FireRate);
        }
    }

    void Fire()     //총알을 스폰한다.
    {
        if (Enable == true)
        {
            switch (BolletDirection)
            {
                case 'U':
                    SetFire(0);
                    break;

                case 'R':
                    SetFire(1);
                    break;

                case 'D':
                    SetFire(2);
                    break;

                case 'L':
                    SetFire(3);
                    break;
            }
        }
    }

    void SetFire(int index)     //총알을 발사할때 작동하는 이벤트 함수
    {
        GameObject Temp;
        Vector2[] Vector = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

        Temp = Instantiate(Bollet, transform.GetChild(index).position, Quaternion.identity);
        Temp.GetComponent<Bollet>().GunBlockID = transform.gameObject.GetInstanceID();
        Temp.GetComponent<Bollet>().Direction = transform.GetChild(index).TransformDirection(Vector[index]);
        Temp.GetComponent<Bollet>().Speed = BolletSpeed;

		GameObject MusicPlayer = Instantiate(Resources.Load("Block/Object/SoundPlayer") as GameObject, new Vector2(0, 0), Quaternion.identity);
		MusicPlayer.GetComponent<SoundPlayer>().Music = Resources.Load("Sound/gun") as AudioClip;
		MusicPlayer.GetComponent<SoundPlayer>().PlayAndDestroyMe();
	}

}
