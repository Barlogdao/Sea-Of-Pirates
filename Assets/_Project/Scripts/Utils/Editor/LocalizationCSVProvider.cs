using Lean.Localization;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Project.Utils.Editor
{
    public class LocalizationCSVProvider : MonoBehaviour
    {
        private const int Token = 0;
        private const int RU = 1;
        private const int EN = 2;
        private const int TR = 3;

        private const char CellSeparator = ',';
        private const char LineEndings = '\n';

        private readonly string _path = Application.dataPath + "/_Project/Resources/Localization/";
        private readonly string _folderPath = "Localization/";

        private readonly string _fileName = "LocalizationSheet";
        [SerializeField] private string _sheetID;
        [SerializeField] private LeanLanguageCSV _russianCSV;
        [SerializeField] private LeanLanguageCSV _englishCSV;
        [SerializeField] private LeanLanguageCSV _turkishCSV;

        private CSVLoader _loader = new();

        [ExecuteInEditMode]
        [ContextMenu("Create")]
        public void CreateCSV()
        {
            _loader.DownloadTable(_sheetID, Process);
        }

        [ExecuteInEditMode]
        [ContextMenu("Update")]
        public void LoadToLean()
        {
            _russianCSV.LoadFromSource();
            _englishCSV.LoadFromSource();
            _turkishCSV.LoadFromSource();
        }

        private void Process(string rawCSV)
        {
            string[] rows = rawCSV.Split(LineEndings);

            LoadCurrentLanguageCSV(_russianCSV, RU);
            LoadCurrentLanguageCSV(_englishCSV, EN);
            LoadCurrentLanguageCSV(_turkishCSV, TR);

            void LoadCurrentLanguageCSV(LeanLanguageCSV leanCSV, int langColumnIndex)
            {
                string csvText = string.Empty;

                for (int i = 0; i < rows.Length; i++)
                {
                    string[] cells = rows[i].Split(CellSeparator);
                    string translation = cells[Token] + CellSeparator + cells[langColumnIndex];

                    if (i < rows.Length - 1)
                    {
                        translation += LineEndings;
                    }

                    csvText += translation;
                }

                string assetName = _fileName + "_" + leanCSV.Language;

                TextAsset asset = ConvertStringToTextAsset(csvText, assetName);

                leanCSV.Source = asset;
                leanCSV.LoadFromSource();
            }
        }

        private TextAsset ConvertStringToTextAsset(string text, string fileName)
        {
            File.WriteAllText(_path + fileName + ".csv", text);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            TextAsset textAsset = Resources.Load(_folderPath + fileName) as TextAsset;
            return textAsset;
        }
    }
}