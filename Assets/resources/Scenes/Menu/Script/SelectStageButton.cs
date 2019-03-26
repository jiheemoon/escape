using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageButton : MonoBehaviour
{
	public void Button_Clicked()
    {
        int StageCount = 10;

        for(int i=0;i<StageCount; i++)
        {
            if(transform.name == "Room " + Convert.ToChar(i + 65))
            {
                PlayerPrefs.SetInt("SelectedStage", i);
                SceneManager.LoadScene("SelectRoom");
            }
        }
    }
}
