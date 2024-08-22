﻿using System;
using Project.Configs.Quests;
using Project.Interfaces.Data;
using Project.Interfaces.Enemies;
using Project.Players.PlayerLogic;
using Project.Systems.Interactables;
using Project.UI.Quests;
using UnityEngine;
using Zenject;

namespace Project.Systems.Quests
{
    public class QuestGiver : InteractableZone
    {
        [SerializeField] private QuestConfig _questConfig;
        [SerializeField] private QuestMarker _questMarker;

        private IEnemyDeathNotifier _enemyDeathNotifier;
        private IPlayerStorage _playerStorage;
        private QuestView _questView;
        private Quest _quest;

        public event Action<int, QuestStatus> QuestStatusChanged;

        public int QuestID => _questConfig.ID;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                if (_quest.Status.State != QuestState.Completed)
                    _questView.Show(_quest);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _questView.Hide();
            }
        }

        private void OnDestroy()
        {
            _quest.StatusChanged -= OnQuestStatusChanged;
            _quest.Unsubscribe();
        }

        [Inject]
        public void Construct(
            IEnemyDeathNotifier enemyDeathNotifier,
            IPlayerStorage playerStorage,
            QuestView questView)
        {
            _enemyDeathNotifier = enemyDeathNotifier;
            _playerStorage = playerStorage;
            _questView = questView;
        }

        public void Initialize(QuestStatus status)
        {
            _quest = new Quest(_questConfig, status, _enemyDeathNotifier);
            _questMarker.SetMarkerVisual(status.State);

            _quest.StatusChanged += OnQuestStatusChanged;
        }

        private void OnQuestStatusChanged(QuestStatus status)
        {
            if (status.State == QuestState.Completed)
            {
                _playerStorage.AddResource(_questConfig.Reward);
                _questView.Hide();
            }

            _questMarker.SetMarkerVisual(status.State);

            QuestStatusChanged?.Invoke(QuestID, status);
        }
    }
}
