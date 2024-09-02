using System;
using System.Collections.Generic;
using System.Linq;
using Project.Spawner;
using Zenject;

namespace Project.Systems.Quests
{
    public class QuestEnemyMarker : IInitializable, IDisposable
    {
        private readonly Dictionary<Quest, List<EnemySpawner>> _questEnemySpawners = new();
        private readonly List<EnemySpawner> _spawners;
        private readonly QuestSystem _questSystem;

        public QuestEnemyMarker(List<EnemySpawner> spawners, QuestSystem questSystem)
        {
            _spawners = spawners;
            _questSystem = questSystem;
        }

        public void Initialize()
        {
            foreach (Quest quest in _questSystem.GetQuests())
            {
                if (_spawners.Any(s => s.EnemyType == quest.Config.TargetType))
                {
                    var questSpawners = _spawners.Where(s => s.EnemyType == quest.Config.TargetType).ToList();
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
                spawner.MarkEnemies();
            }
        }

        private void UnmarkQuestEnemies(Quest quest)
        {
            foreach (var spawner in _questEnemySpawners[quest])
            {
                spawner.UnmarkEnemies();
            }
        }

        private void OnQuestStatusChanged(Quest quest)
        {
            switch (quest.Status.State)
            {
                case QuestState.Taken:
                    MarkQuestEnemies(quest);
                    break;

                case QuestState.Done:
                    UnmarkQuestEnemies(quest);
                    break;

                default:
                    break;
            }
        }
    }
}