using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
	public float PlayerSpeed = 4.3f;            //플레이어 이동속도 
	public float PlayerJumpPower = 30.2f;       //플레이어 점프력
	public int AvailableJumpCount = 1;          //가능한 점프수
	public bool MovingBlock = false;            //음직이는 블럭 true 이면 블럭이 음직임
	public char MovingDirection = 'L';          //블럭의 이동방향
	public float MovingSpeed = 4.3f;            //블럭의 이동속도
	public bool isCracked = false;              //갈라진 블럭? true 이면 플레이어가 블럭을 밟고 블럭에서 나오면 사라짐
	public bool isRotating = false;             //회전하는 블럭인가?...true 면 회전
	public char RotateDirection = 'R';          //회전방향
	public float RotateSpeed = 100f;            //회전속도
	bool isCollied = false;
	bool isBreaked = false;

	GameObject player;                      //선언:게임오프젝트 플레이어

	// Use this for initialization
	void Start()
	{
		player = GameObject.Find("Player"); //플래이어의 게임오프젝트를 가져옴
		transform.gameObject.AddComponent<AudioSource>();
	}

	void Update()   //블럭 이동 처리
	{
		if (MovingBlock)       //MovingBlock 가 활성화 되 있으면
		{
			switch (MovingDirection)
			{
				case 'L':   //MovingDirection = L
					transform.Translate(Vector2.left * MovingSpeed * Time.deltaTime);   //이동
					break;

				case 'R':   //MovingDirection = R
					transform.Translate(Vector2.right * MovingSpeed * Time.deltaTime);  //이동
					break;

				case 'U':   //MovingDirection = T
					transform.Translate(Vector2.up * MovingSpeed * Time.deltaTime);     //이동
					break;

				case 'D':   //MovingDirection = B
					transform.Translate(Vector2.down * MovingSpeed * Time.deltaTime);   //이동
					break;
			}
		}
		if (isRotating) Rotate();           //isRotating 가 true 이면 반복실행
	}

	// Update is called once per frame

	void OnTriggerEnter2D(Collider2D Col)
	{
		if (Col.name == "Player_Foot") SetValue();     //플레이어의 발이 지면에 닿으면  값을 설정

		if (Col.transform.name == "Player_Foot" && isCracked && !isBreaked)
		{
			StartCoroutine(Breakdown());
			isBreaked = true;
		}
	}

	void OnTriggerStay2D(Collider2D Col)
	{
		if (Col.name == "Player_Foot") SetValue();   //플레이어의 발이 지면에 닿으면  값을 설정

		if (Col.transform.name == "Player_Foot" && isCracked && !isBreaked)
		{
			StartCoroutine(Breakdown());
			isBreaked = true;
		}
	}

	IEnumerator Breakdown()                     //밟으면 부셔지는 블록 이벤트 함수
	{
		int VibrateDir = 0;
		float Original_X_Pos = transform.position.x;

		for (int i = 1; i <= 30; i++)     //블록을 좌우로 흔들리게 하는 소스
		{
			if (VibrateDir == 0)
			{
				transform.position = new Vector2(Original_X_Pos + 0.02f, transform.position.y);
				VibrateDir = 1;
			}
			else
			{
				transform.position = new Vector2(Original_X_Pos - 0.02f, transform.position.y);
				VibrateDir = 0;
			}
			yield return new WaitForSeconds(0.01f);
		}

		GameObject MusicPlayer = Instantiate(Resources.Load("Block/Object/SoundPlayer") as GameObject, new Vector2(0, 0), Quaternion.identity);
		MusicPlayer.GetComponent<SoundPlayer>().Music = Resources.Load("Sound/break") as AudioClip;
		MusicPlayer.GetComponent<SoundPlayer>().PlayAndDestroyMe();

		//여기에 소리파일을 넣는걸 추천!

		Destroy(transform.gameObject);          //블록을 파괴하는 소스
	}

	void OnTriggerExit2D(Collider2D Col)
	{
		if (Col.transform.name == "Player_Foot" && player.GetComponent<Player>().Jumping == false)        //플레이어가 점프를 안한상태에서 블럭에서 벗어나면
		{
			player.GetComponent<Player>().AvailableJumpCount = 0;   //점프카운트를 0 으로 만든다.
		}
	}

	void OnCollisionEnter2D(Collision2D Col)
	{
		//Debug.Log(Col.transform.name + "과 충돌됨");
		if (Col.transform.tag == "Block" && !isCollied)     //충돌한 오브잭트의 테그가 블록이면
		{
			isCollied = true;
			switch (MovingDirection)    //블록의 진행방향을 바꾼다.
			{
				case 'L':
					MovingDirection = 'R';
					break;

				case 'R':
					MovingDirection = 'L';
					break;

				case 'U':
					MovingDirection = 'D';
					break;

				case 'D':
					MovingDirection = 'U';
					break;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D Col)
	{
		if (isCollied && Col.transform.tag == "Block") isCollied = false;
	}

	void SetValue()     //플레이어에게 이동속도,점프력,점프 카운트 수를 적용하는 함수
	{
		player.GetComponent<Player>().Speed = PlayerSpeed;
		player.GetComponent<Player>().JumpPower = PlayerJumpPower;
		player.GetComponent<Player>().AvailableJumpCount = AvailableJumpCount;
		player.GetComponent<Player>().Jumping = false;
	}

	void Rotate()
	{
		if (RotateDirection == 'R')
		{
			transform.Rotate(new Vector3(0, 0, -1) * RotateSpeed * Time.deltaTime);
		}
		else if (RotateDirection == 'L')
		{
			transform.Rotate(new Vector3(0, 0, 1) * RotateSpeed * Time.deltaTime);
		}
	}
}
