using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesNoMessageBox : MonoBehaviour
{
    public void ShowMessageBox(string TextContent, string TitleText, UnityEngine.Events.UnityAction YesEventFunc, UnityEngine.Events.UnityAction NoEventFunc)
    {
        transform.Find("Content").Find("Text").GetComponent<Text>().text = TextContent;
        transform.Find("Title").Find("Text").GetComponent<Text>().text = TitleText;
        transform.Find("YesButton").GetComponent<Button>().onClick.AddListener(YesEventFunc);
        transform.Find("NoButton").GetComponent<Button>().onClick.AddListener(NoEventFunc);
        transform.Find("Title").Find("Button").GetComponent<Button>().onClick.AddListener(NoEventFunc);
        transform.gameObject.SetActive(true);
    }
}
