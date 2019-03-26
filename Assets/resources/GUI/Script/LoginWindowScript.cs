using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using NetworkInformation;
using SyncToServer;

public class LoginWindowScript : MonoBehaviour
{
    public void LoginBtn_Clicked()
    {
        string ID = transform.Find("ID").Find("Text").GetComponent<Text>().text;
        StartCoroutine(Login(ID));
    }

    IEnumerator Login(string ID)
    {
        GetMac Mac = new GetMac();

        Synchronizer WebRequest = new Synchronizer();
        DataField SendData = new DataField() { CMD = CommandList.Login, ID = ID, MAC = Mac.MacAddress };
        StartCoroutine(WebRequest.Send(SendData));
        while (WebRequest.Status == 0) yield return new WaitForSeconds(0.1f);

        if (WebRequest.Status == 1)
        {
            string GetData = WebRequest.ResponceText;

            if (int.Parse(GetData) == 1)        //기존 계정에 재 로그인시
            {
                PlayerPrefs.SetString("ID", ID);
                PlayerPrefs.Save();             //계정 정보를 저장
                GameObject Msgbox = Instantiate(Resources.Load("GUI/Window/MessageBox", typeof(GameObject)), GameObject.Find("Canvas").transform) as GameObject;
                Msgbox.GetComponent<MessageBoxScript>().ShowMessageBox("이미 존재하는 계정에 재등록 하셨습니다...재등록 성공", "메시지");
                GameObject.Find("GameStartBtn").GetComponent<Button>().interactable = true;
                GameObject.Find("HelpBtn").GetComponent<Button>().interactable = true;
                GameObject.Find("ExitGameBtn").GetComponent<Button>().interactable = true;
                Destroy(transform.gameObject);
            }
            else if (int.Parse(GetData) == 0)    //기존 계정에 재로그인 실패시
            {
                GameObject Msgbox = Instantiate(Resources.Load("GUI/Window/MessageBox", typeof(GameObject)), GameObject.Find("Canvas").transform) as GameObject;
                Msgbox.GetComponent<MessageBoxScript>().ShowMessageBox("이미 존재하는 닉네임 입니다", "오류");
            }
            else if (int.Parse(GetData) == -1)   //새로운 계정 입력시
            {
                yield return StartCoroutine(SignUp(ID));
            }
        }
    }

    IEnumerator SignUp(string ID)
    {
        GetMac Mac = new GetMac();
        Synchronizer WebRequest = new Synchronizer();
        DataField SendData = new DataField() { CMD = CommandList.RegistID, ID = ID, MAC = Mac.MacAddress };
        StartCoroutine(WebRequest.Send(SendData));
        while (WebRequest.Status == 0) yield return new WaitForSeconds(0.1f);

        if (WebRequest.Status == 1)
        {
            string GetData = WebRequest.ResponceText;

            if (int.Parse(GetData) == 1)
            {
                PlayerPrefs.SetString("ID", ID);
                PlayerPrefs.Save();             //계정 정보를 저장
                GameObject Msgbox = Instantiate(Resources.Load("GUI/Window/MessageBox", typeof(GameObject)), GameObject.Find("Canvas").transform) as GameObject;
                Msgbox.GetComponent<MessageBoxScript>().ShowMessageBox("등록 완료", "메시지");
                GameObject.Find("GameStartBtn").GetComponent<Button>().interactable = true;
                GameObject.Find("HelpBtn").GetComponent<Button>().interactable = true;
                GameObject.Find("ExitGameBtn").GetComponent<Button>().interactable = true;
                Destroy(transform.gameObject);
            }
            else if (int.Parse(GetData) == 0)
            {
                GameObject Msgbox = Instantiate(Resources.Load("GUI/Window/MessageBox", typeof(GameObject)), GameObject.Find("Canvas").transform) as GameObject;
                Msgbox.GetComponent<MessageBoxScript>().ShowMessageBox("이미 존재하는 닉네임 입니다...등록 오류", "오류");
            }
            else if (int.Parse(GetData) == -1)
            {
                GameObject Msgbox = Instantiate(Resources.Load("GUI/Window/MessageBox", typeof(GameObject)), GameObject.Find("Canvas").transform) as GameObject;
                Msgbox.GetComponent<MessageBoxScript>().ShowMessageBox("시스템 오류...", "오류");
            }
        }
    }
}
