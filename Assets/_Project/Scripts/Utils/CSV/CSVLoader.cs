using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Project.Utils.CSV
{
    public class CSVLoader
    {
        private const string UrlPattern = "https://docs.google.com/spreadsheets/d/*/export?format=csv";

        public void DownloadTable(string sheetID, Action<string> onSheetLoadedAction)
        {
            string actualURL = UrlPattern.Replace("*", sheetID);
            DownloadCSVTable(actualURL, onSheetLoadedAction).Forget();
        }
        private async UniTaskVoid DownloadCSVTable(string actualUrl, Action<string> callback)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(actualUrl))
            {
                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError ||
                    request.result == UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    callback(request.downloadHandler.text);
                }
            }
        }
    }
}