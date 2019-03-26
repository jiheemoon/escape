using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        PlayerPrefs.DeleteAll();
        string UserID = PlayerPrefs.GetString("ID");	

        if(UserID == "")
        {
            Instantiate(Resources.Load("GUI/Window/LoginWindow"),GameObject.Find("Canvas").transform);
        }
        else
        {
            GameObject.Find("GameStartBtn").GetComponent<Button>().interactable = true;
            GameObject.Find("HelpBtn").GetComponent<Button>().interactable = true;
            GameObject.Find("ExitGameBtn").GetComponent<Button>().interactable = true;
        }
    }

    public void GameStartBtn_Clicked()
    {
        SceneManager.LoadScene("SelectStage");
    }

    public void HelpBtn_Clicked()
    {
        SceneManager.LoadScene("HelpScene");
    }

    public void ExitBtn_Clicked()
    {
        Application.Quit();
    }

}
