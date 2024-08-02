using System.IO;
using UnityEditor;
using UnityEngine;

namespace Project.Utils.CSV
{
    public static class AssetCreator
    {
        public static TextAsset ConvertStringToTextAsset(string path, string folderPath, string text, string fileName)
        {
            File.WriteAllText(path + fileName + ".csv", text);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            TextAsset textAsset = Resources.Load(folderPath + fileName) as TextAsset;

            return textAsset;
        }
    }
}