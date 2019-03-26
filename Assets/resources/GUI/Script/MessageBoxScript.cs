using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxScript : MonoBehaviour
{
    public void ShowMessageBox(string TextContent,string TitleText)
    {
        transform.Find("Content").Find("Text").GetComponent<Text>().text = TextContent;
        transform.Find("Title").Find("Text").GetComponent<Text>().text = TitleText;
        transform.gameObject.SetActive(true);
    }

    public void CloseBtn_Clicked()
    {
        Destroy(transform.gameObject);
    }
}
