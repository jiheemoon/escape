using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectStage : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        int StageCount = 10;
        int StageData = PlayerPrefs.GetInt("StageData");

        if(StageData == 0)
        {
            PlayerPrefs.SetInt("StageData", 1);
            StageData = 1;
        }

        for (int i = (StageData/15) + 1; i < StageCount; i++)
        {
            GameObject.Find("Room " + Convert.ToChar(i + 65)).GetComponent<Button>().interactable = false;
        }
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
