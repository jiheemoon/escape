using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectRoomButton : MonoBehaviour
{
    public void RoomButton_Clicked()
    {
        int RoomCount = 15;
        int SelectedStage = PlayerPrefs.GetInt("SelectedStage");
        int StageData = PlayerPrefs.GetInt("StageData");

        for (int i = 0; i < RoomCount; i++)
        {
            if (transform.name == i.ToString())
            {
                PlayerPrefs.SetInt("SelectedRoom", i);
                SceneManager.LoadScene(Convert.ToChar(SelectedStage + 65) + "-" + i);
            }
        }
    }
	
}
