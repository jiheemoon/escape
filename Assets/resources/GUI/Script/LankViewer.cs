using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SyncToServer;

public class LankViewer : MonoBehaviour
{
    
    public void CloseFormBtn_Clicked()
    {
        SceneManager.LoadScene("SelectRoom");
    }

    public void ShowLankViewer(string UserID,string Stage)
    {
        int ClearTime = GameObject.Find("GameManager").GetComponent<GameManager>().ClearTime;
        int H = 0, M = 0, S = 0;

        H = ClearTime / 3600;
        M = (ClearTime % 3600) / 60;
        S = (ClearTime % 3600) % 60;

        transform.gameObject.SetActive(true);
        StartCoroutine(GetTimeRecordAndLank(UserID, Stage));
        StartCoroutine(GetTimeRecordTable(Stage, Resources.Load("Font/Arial") as Font));
        transform.Find("TimeRecord").GetComponent<Text>().text = H + "시 " + M + "분 " + S + "초 = " + ClearTime + "(초)";
        transform.Find("Top").Find("Title").GetComponent<Text>().text = "전체 랭킹 (플레이어:" + PlayerPrefs.GetString("ID") + ")";
    }

    IEnumerator GetTimeRecordAndLank(string ID, string StageName)
    {
        DataField SendData = new DataField() { CMD=CommandList.GetTimeRecordAndLank,ID=ID,STAGE=StageName};     //전송할 데이터 입력
        Synchronizer WebRequest = new Synchronizer();

        StartCoroutine(WebRequest.Send(SendData));   //데이터 전송
        while(WebRequest.Status == 0) yield return new WaitForSeconds(1);    //전송 상태 를 응답 받을때 까지 대기

        if(WebRequest.Status == 1)   //데이터 전송에 성공했다면
        {
            if (WebRequest.ResponceText.Contains(","))
            {
                int TimeRecord = int.Parse(WebRequest.ResponceText.Split(',')[0]);
                int Lank = int.Parse(WebRequest.ResponceText.Split(',')[1]);
                int H = 0, M = 0, S = 0;

                H = TimeRecord / 3600;
                M = (TimeRecord % 3600) / 60;
                S = (TimeRecord % 3600) % 60;

                transform.Find("BestTimeRecord").GetComponent<Text>().text = H + "시 " + M + "분 " + S + "초 = " + TimeRecord + "(초)";
                transform.Find("LankNum").GetComponent<Text>().text = Lank.ToString() + "위";
            }
            else
            {
                transform.Find("BestTimeRecord").GetComponent<Text>().text = "데이터 없음";
                transform.Find("LankNum").GetComponent<Text>().text = "데이터 없음";
            }
        }
        yield return null;
    }

    IEnumerator GetTimeRecordTable(string StageName,Font TableFont)
    {
        DataField SendData = new DataField() { CMD = CommandList.GetTimeRecordTable, STAGE = StageName,RANGE0=0,RANGE1=30};
        Synchronizer WebRequest = new Synchronizer();
        StartCoroutine(WebRequest.Send(SendData));   //데이터 전송
        while (WebRequest.Status == 0) yield return new WaitForSeconds(1);

        if (WebRequest.Status == 1)
        {
            string Data = WebRequest.ResponceText;

            if (Data != "-1")
            {
                string[] Table = Data.Split(',');
                int j = 1;
                for (int i = 0; i <= Table.Length - 1; i++)
                {
                    if (i % 4 == 0)
                    {
                        if(i >= 4)
                        {
                            if (Table[i - 2] == Table[i + 2])
                            {
                                FillTable(transform.Find("Scroll View").transform.Find("Viewport").Find("Content"), 20, 20, j.ToString(), 12, Color.black, TableFont);
                            }
                            else
                            {
                                j++;
                                FillTable(transform.Find("Scroll View").transform.Find("Viewport").Find("Content"), 20, 20, j.ToString(), 12, Color.black, TableFont);
                            }
                        }
                        else
                        {
                            FillTable(transform.Find("Scroll View").transform.Find("Viewport").Find("Content"), 20, 20, j.ToString(), 12, Color.black, TableFont);
                        }
                        
                    }
                    FillTable(transform.Find("Scroll View").transform.Find("Viewport").Find("Content"), 20, 20, Table[i], 12, Color.black, TableFont);
                }
            }
            else
            {
                Debug.Log("테이블 가져오기 오류 발생");
            }
        }
    }

    GameObject FillTable(Transform Parent, float x, float y, string text_to_print, int font_size, Color text_color,Font TableFont)
    {
        GameObject UItextGO = new GameObject("Cell");
        UItextGO.transform.SetParent(Parent);

        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.anchoredPosition = new Vector2(x, y);

        Text text = UItextGO.AddComponent<Text>();
        text.text = text_to_print;
        text.fontSize = font_size;
        text.color = text_color;
        text.font = TableFont;
        text.alignment = TextAnchor.MiddleCenter;

        return UItextGO;
    }
}
