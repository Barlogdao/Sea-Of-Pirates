using Lean.Localization;
using NaughtyAttributes;
using UnityEngine;


namespace Project.Utils.CSV
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
        private readonly string _assetFolder = "Localization/";
        private readonly string _fileName = "LocalizationSheet";

        [SerializeField] private string _sheetID;
        [SerializeField] private LeanLanguageCSV _russianCSV;
        [SerializeField] private LeanLanguageCSV _englishCSV;
        [SerializeField] private LeanLanguageCSV _turkishCSV;

        [ExecuteInEditMode]
        [Button("Update Localization",EButtonEnableMode.Editor)]
        public void UpdateCSV()
        {
            CSVLoader loader = new CSVLoader();

            loader.DownloadTable(_sheetID, Process);
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

                TextAsset asset = AssetCreator.ConvertStringToTextAsset(_path, _assetFolder, csvText, assetName);

                leanCSV.Source = asset;
                leanCSV.LoadFromSource();
            }
        }
    }
}