using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SyncToServer;
using NetworkInformation;

public class GameManager : MonoBehaviour
{
    GameObject PlayerGameObj;
    public int ClearTime = 0;
    int H = 0, M = 0, S = 0;
    public bool GameOver = false;
    public string ThisStage;
    public string PlayerID;

    void Start ()//타이머 설정부분 , 플레이어 게임 오브젝트 잡아주기
    {
        InvokeRepeating("Timer", 0, 1);
        PlayerGameObj = GameObject.Find("Player");
        ThisStage = Convert.ToChar(PlayerPrefs.GetInt("SelectedStage") / 15 + 65) + "-" + (PlayerPrefs.GetInt("SelectedRoom"));
        PlayerID = PlayerPrefs.GetString("ID");
        //Debug.Log(PlayerID);
    } 

    void ShowGameOverMsgbox() //게임오버 메시지 박스를 보여주는 함수
    {
        GameObject YesNoMsgbox = Instantiate(Resources.Load("GUI/Window/YesNoMessageBox", typeof(GameObject)), GameObject.Find("Canvas").transform) as GameObject;
        YesNoMsgbox.GetComponent<YesNoMessageBox>().ShowMessageBox("게임오버...게속 하시겠습니까?", "게임오버", ResetGame, ExitGame);
    }  

    public void ResetGame() //게임을 재시작 하는 함수
    {
        SceneManager.LoadScene(ThisStage);
    }

    public void ExitGame() //게임을 종료시키는 함수 
    {
        SceneManager.LoadScene("SelectRoom");
    } 

    public IEnumerator GameClear() //게임 클리어시 작동하는 함수
    {
        if (!GameOver)
        {
            GameOver = true;            //게임 오버 선언
            CancelInvoke("Timer");      //타이머 정지

            //서버와 통신하여 플레이어의 스테이지 점수를 가져옴 
            DataField SendData = new DataField() { CMD = CommandList.GetTimeRecordAndLank, STAGE = ThisStage, ID = PlayerID };
            Synchronizer WebRequest = new Synchronizer();
            StartCoroutine(WebRequest.Send(SendData));   //데이터 전송
            while (WebRequest.Status == 0) yield return new WaitForSeconds(0.1f);    //전송 상태 를 응답 받을때 까지 대기

            if (WebRequest.Status == 1)  //서버로부터 데이터를 받아오는데 성공하면
            {
                if (WebRequest.ResponceText.Contains(","))  //서버에 플레이어의 점수가 있다면
                {
                    
                    int BestTimeRecord = int.Parse(WebRequest.ResponceText.Split(',')[0]);  //최고기록 파싱
                    if (ClearTime < BestTimeRecord)    //클리어 시간이 플레이어의 최고기록시간 보다 적다면
                    {
                        //서버에 방금 얻은 기록을 갱신한다.
                        SendData = new DataField() { CMD = CommandList.UploadScore, STAGE = ThisStage, ID = PlayerID, MAC = new GetMac().MacAddress, TIMERECORD = ClearTime };
                        WebRequest = new Synchronizer();
                        StartCoroutine(WebRequest.Send(SendData));   //데이터 전송
                        while (WebRequest.Status == 0) yield return new WaitForSeconds(0.1f);    //전송 상태 를 응답 받을때 까지 대기

                        if (WebRequest.Status == 1)  //서버에 점수 업로드 성공시
                        {
                            GameObject LankViewerForm = Instantiate(Resources.Load("GUI/Window/LankViewer"), GameObject.Find("Canvas").transform) as GameObject;
                            LankViewerForm.GetComponent<LankViewer>().ShowLankViewer(PlayerID, ThisStage);

                            //Debug.Log("성공: " + WebRequest.ResponceText);
                            GameObject Msgbox = Instantiate(Resources.Load("GUI/Window/MessageBox", typeof(GameObject)), GameObject.Find("Canvas").transform) as GameObject;
                            Msgbox.GetComponent<MessageBoxScript>().ShowMessageBox("축하합니다 최고기록을 갱신 하셨군요!!", "메시지");
                        }
                        else                      //서버에 점수 업로드 실패시
                        {
                            GameObject LankViewerForm = Instantiate(Resources.Load("GUI/Window/LankViewer"), GameObject.Find("Canvas").transform) as GameObject;
                            LankViewerForm.GetComponent<LankViewer>().ShowLankViewer(PlayerID, ThisStage);

                            //Debug.Log("실패: " + WebRequest.ResponceText);
                            GameObject Msgbox = Instantiate(Resources.Load("GUI/Window/MessageBox", typeof(GameObject)), GameObject.Find("Canvas").transform) as GameObject;
                            Msgbox.GetComponent<MessageBoxScript>().ShowMessageBox("통신 오류가 발생하였습니다...", "오류");
                        }

                    }
                    else
                    {
                        SendData = new DataField() { CMD = CommandList.UploadScore, STAGE = ThisStage, ID = PlayerID, MAC = new GetMac().MacAddress, TIMERECORD = ClearTime };
                        WebRequest = new Synchronizer();
                        StartCoroutine(WebRequest.Send(SendData));
                        while (WebRequest.Status == 0) yield return new WaitForSeconds(0.1f);

                        if (WebRequest.Status == 1)  //서버에 점수 업로드 성공시
                        {
                            //Debug.Log("성공: " + WebRequest.ResponceText);
                            GameObject LankViewerForm = Instantiate(Resources.Load("GUI/Window/LankViewer"), GameObject.Find("Canvas").transform) as GameObject;
                            LankViewerForm.GetComponent<LankViewer>().ShowLankViewer(PlayerID, ThisStage);
                        }
                        else
                        {
                            //Debug.Log("실패: " + WebRequest.ResponceText);
                            GameObject Msgbox = Instantiate(Resources.Load("GUI/Window/MessageBox", typeof(GameObject)), GameObject.Find("Canvas").transform) as GameObject;
                            Msgbox.GetComponent<MessageBoxScript>().ShowMessageBox("통신 오류가 발생하였습니다...", "오류");
                        }
                    }

                }
                else
                {
                    SendData = new DataField() { CMD = CommandList.UploadScore, STAGE = ThisStage, ID = PlayerID, MAC = new GetMac().MacAddress, TIMERECORD = ClearTime };
                    WebRequest = new Synchronizer();
                    StartCoroutine(WebRequest.Send(SendData));
                    while (WebRequest.Status == 0) yield return new WaitForSeconds(0.1f);

                    if (WebRequest.Status == 1)  //서버에 점수 업로드 성공시
                    {
                        //Debug.Log("성공: " + WebRequest.ResponceText);
                        GameObject LankViewerForm = Instantiate(Resources.Load("GUI/Window/LankViewer"), GameObject.Find("Canvas").transform) as GameObject;
                        LankViewerForm.GetComponent<LankViewer>().ShowLankViewer(PlayerID, ThisStage);
                    }
                    else
                    {
                        //Debug.Log("실패: " + WebRequest.ResponceText);
                        GameObject Msgbox = Instantiate(Resources.Load("GUI/Window/MessageBox", typeof(GameObject)), GameObject.Find("Canvas").transform) as GameObject;
                        Msgbox.GetComponent<MessageBoxScript>().ShowMessageBox("통신 오류가 발생하였습니다...", "오류");
                    }
                }
                //Debug.Log(PlayerID + " " + ThisStage);
            }
            else
            {
                Debug.Log("통신 실패");
            }

            //스테이지 언락 부분
            int StageData = PlayerPrefs.GetInt("StageData");
            int SelectedStage = PlayerPrefs.GetInt("SelectedStage");
            int SelectedRoom = PlayerPrefs.GetInt("SelectedRoom");

            if(StageData <= (SelectedStage*15)+SelectedRoom)
            {
                PlayerPrefs.SetInt("StageData", ++StageData);
            }
        }
    }  

    void Timer()  //플레이 시간을 기록하는 타이머 함수
    {
        ClearTime++;
        H = ClearTime / 3600;
        M = (ClearTime % 3600) / 60;
        S = (ClearTime % 3600) % 60;
        GameObject.Find("PlayTime").GetComponent<Text>().text = "플레이 시간: ";
        GameObject.Find("PlayTime").GetComponent<Text>().text += H + "시 " + M + "분 " + S + "초";
    }

    IEnumerator Player_Dead_Emotion() //플레이어가 죽는 이모션
    {

        PlayerGameObj.transform.GetComponent<CapsuleCollider2D>().enabled = false;
        PlayerGameObj.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        PlayerGameObj.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        PlayerGameObj.transform.GetComponent<Rigidbody2D>().simulated = false;
        yield return new WaitForSeconds(0.5f);
        PlayerGameObj.transform.GetComponent<Rigidbody2D>().simulated = true;
        PlayerGameObj.transform.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 280);
        Invoke("ShowGameOverMsgbox", 2);

		GameObject MusicPlayer = Instantiate(Resources.Load("Block/Object/SoundPlayer") as GameObject, new Vector2(0, 0), Quaternion.identity);
		MusicPlayer.GetComponent<SoundPlayer>().Music = Resources.Load("Sound/die") as AudioClip;
		MusicPlayer.GetComponent<SoundPlayer>().PlayAndDestroyMe();
	}

    void Update()   //게임 오버 감지
    {
		if(PlayerGameObj.GetComponent<Player>().Helth == 0 && !GameOver)    //플레이어가 게임오버되면 발생함
        {
            GameOver = true;
            CancelInvoke("Timer");
            StartCoroutine(Player_Dead_Emotion());
        }
	}
}
