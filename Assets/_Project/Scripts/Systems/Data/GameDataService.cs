using System.Collections.Generic;
using Project.Configs.Game;
using Project.Interfaces.Data;
using UnityEngine;

namespace Project.Systems.Data
{
    public class GameDataService : IResourceStorageData,
        IPlayerStatsData,
        IQuestsData,
        IAdvertismentData,
        ILevelSceneService
    {
        private const string SaveKey = nameof(SaveKey);

        private readonly GameData _gameData;

        public GameDataService(GameConfig config)
        {
            GameData data = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString(SaveKey, null));

            _gameData = data ?? new GameData(config.FirstLevelScene);
        }

        public List<GameResourceData> Storage => _gameData.StorageData;

        public List<PlayerStatData> StatsLevels => _gameData.PlayerStatsLevels;

        public List<QuestData> Quests => _gameData.Quests;

        public string CurrentLevel => _gameData.CurrentScene;

        public bool IsAdActive { get => _gameData.IsAddActive; set => _gameData.IsAddActive = value; }

        public void UpdateCurrentLevel(string levelName)
        {
            _gameData.CurrentScene = levelName;
            Save();
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(_gameData);

            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }
    }
}