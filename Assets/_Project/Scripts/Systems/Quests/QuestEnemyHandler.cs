using System;
using System.Collections.Generic;
using System.Linq;
using Project.Spawner;
using Project.Systems.Cameras;
using UnityEngine;
using Zenject;

namespace Project.Systems.Quests
{
    public class QuestEnemyHandler : IInitializable, IDisposable
    {
        private const float SpawnerShowDuration = 3f;

        private readonly Dictionary<Quest, List<BaseEnemySpawner>> _questEnemySpawners = new();
        private readonly List<BaseEnemySpawner> _spawners;
        private readonly QuestSystem _questSystem;
        private readonly CameraSystem _cameraSystem;

        public QuestEnemyHandler(List<BaseEnemySpawner> spawners, QuestSystem questSystem, CameraSystem cameraSystem)
        {
            _spawners = spawners;
            _questSystem = questSystem;
            _cameraSystem = cameraSystem;
        }

        public void Initialize()
        {
            foreach (Quest quest in _questSystem.GetQuests())
            {
                if (_spawners.Any(spawner => spawner.EnemyType == quest.Config.TargetType))
                {
                    List<BaseEnemySpawner> questSpawners = _spawners.Where(s => s.EnemyType == quest.Config.TargetType).ToList();
                    _questEnemySpawners.Add(quest, questSpawners);
                }
            }

            foreach (var quest in _questEnemySpawners.Keys)
            {
                quest.StatusChanged += OnQuestStatusChanged;
            }

            InitializeQuestEnemies();
        }

        private void InitializeQuestEnemies()
        {
            foreach (Quest quest in _questEnemySpawners.Keys)
            {
                QuestState state = quest.Status.State;

                if (state == QuestState.Taken || state == QuestState.InProgress)
                    MarkQuestEnemies(quest);
            }
        }

        public void Dispose()
        {
            foreach (var quest in _questEnemySpawners.Keys)
            {
                quest.StatusChanged -= OnQuestStatusChanged;
            }
        }

        private void MarkQuestEnemies(Quest quest)
        {
            foreach (var spawner in _questEnemySpawners[quest])
            {
                spawner.PrepareForQuest();
            }
        }

        private void UnmarkQuestEnemies(Quest quest)
        {
            foreach (var spawner in _questEnemySpawners[quest])
            {
                spawner.ReleaseFromQuest();
            }
        }

        private void OnQuestStatusChanged(Quest quest)
        {
            switch (quest.Status.State)
            {
                case QuestState.Taken:
                    MarkQuestEnemies(quest);
                    ShowQuestSpawner(quest);
                    break;

                case QuestState.Done:
                    UnmarkQuestEnemies(quest);
                    break;

                default:
                    break;
            }
        }

        private void ShowQuestSpawner(Quest quest)
        {
            Transform questSpawner = _questEnemySpawners[quest].First().transform;

            _cameraSystem.ShowTarget(questSpawner, SpawnerShowDuration).Forget();
        }
    }
}