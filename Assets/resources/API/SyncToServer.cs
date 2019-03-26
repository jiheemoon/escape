using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;

namespace SyncToServer
{
    public class Synchronizer : MonoBehaviour
    {
        public string URL = "http://phpserver876454.esy.es/LankManager.php?";
        public string ResponceText;
        public int Status = 0;      //-1 faild , 0 = not connected , 1 = success

        public IEnumerator Send(DataField Field)
        {
            Status = 0;
            WWWForm form = new WWWForm();
            form.AddField("ID", Field.ID);
            form.AddField("CMD", Field.CMD);
            form.AddField("MAC", Field.MAC);
            form.AddField("STAGE", Field.STAGE);
            form.AddField("TIMERECORD", Field.TIMERECORD);
            form.AddField("RANGE0", Field.RANGE0);
            form.AddField("RANGE1", Field.RANGE1);

            UnityWebRequest www = UnityWebRequest.Post(URL, form);
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Status = -1;
            }
            else
            {
                ResponceText = www.downloadHandler.text;
                Status = 1;
            }
        }
    }

    public class DataField
    {
        public string ID = string.Empty;
        public int CMD = 0;
        public string MAC = string.Empty;
        public string STAGE = string.Empty;
        public int TIMERECORD = 0;
        public int RANGE0 = 0;
        public int RANGE1 = 0;
    }

    public struct CommandList
    {
        public const int Login = 0;
        public const int RegistID = 1;
        public const int UnRegistID = 2;
        public const int UploadScore = 3;
        public const int GetTimeRecordAndLank = 4;
        public const int GetTimeRecordTable = 5;
    }
}