using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpScene : MonoBehaviour
{
    public void GotoMainBtn_Clicked()
    {
        SceneManager.LoadScene("Main");
    }
}
