using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SyncToServer;

public class SelectRoom : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        int RoomCount = 15;
        int SelectedStage = PlayerPrefs.GetInt("SelectedStage");
        int StageData = PlayerPrefs.GetInt("StageData");
        for (int i = 0; i < RoomCount; i++)
        {
            GameObject.Find("Buttons").transform.GetChild(i).Find("Text").GetComponent<Text>().text = Convert.ToChar(SelectedStage + 65) + "-" + (i+1);
            if((StageData < (SelectedStage * 15) + i + 1))
            {
                GameObject.Find("Buttons").transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            else
            {
                string RoomName = Convert.ToChar(SelectedStage + 65) + "-" + (i + 1).ToString();
                StartCoroutine(GetRoomScore(RoomName, GameObject.Find("Buttons").transform.GetChild(i).GetComponent<Button>()));
            }
        }
	}

    IEnumerator GetRoomScore(string Room,Button RoomButton)
    {
        Synchronizer Socket = new Synchronizer();
        DataField SendData = new DataField()
        {
            CMD = CommandList.GetTimeRecordAndLank,
            ID = PlayerPrefs.GetString("ID"),
            STAGE = Room,
        };
        StartCoroutine(Socket.Send(SendData));
        while (Socket.Status == 0) yield return new WaitForSeconds(0.0001f);

        if(Socket.ResponceText.Contains(","))
        {
            int Score = int.Parse(Socket.ResponceText.Split(',')[0]);
            if(Score <= 45)
            {
                RoomButton.transform.Find("Text").GetComponent<Text>().text += "\r\n" + "★★★" + "\r\n" + Score.ToString() + "초";
            }
            else if(45 < Score && Score <= 115)
            {
                RoomButton.transform.Find("Text").GetComponent<Text>().text += "\r\n" + "★★☆" + "\r\n" + Score.ToString() + "초";
            }
            else if(115 < Score)
            {
                RoomButton.transform.Find("Text").GetComponent<Text>().text += "\r\n" + "★☆☆" + "\r\n" + Score.ToString() + "초";
            }
        }
        else
        {
            RoomButton.transform.Find("Text").GetComponent<Text>().text += "\r\n" + "☆☆☆";
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("SelectStage");
        }
    }
}
