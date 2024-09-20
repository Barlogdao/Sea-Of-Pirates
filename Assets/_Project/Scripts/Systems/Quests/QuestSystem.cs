using System;
using System.Collections.Generic;
using System.Linq;
using Ami.BroAudio;
using Project.Configs.Game;
using Project.Interfaces.Audio;
using Project.Interfaces.Data;
using UnityEngine;
using Zenject;

namespace Project.Systems.Quests
{
    public class QuestSystem : IDisposable, IInitializable
    {
        private readonly IQuestsProvider _questsProvider;
        private readonly IAudioService _audioService;
        private readonly List<QuestGiver> _questGivers;
        private readonly SoundID _questDoneSound;

        public QuestSystem(
            List<QuestGiver> questGivers,
            IQuestsProvider questsProvider,
            IAudioService audioService,
            GameConfig config)
        {
            _questsProvider = questsProvider;
            _questGivers = questGivers;
            _audioService = audioService;
            _questDoneSound = config.QuestDoneSound;
        }

        public void Dispose()
        {
            foreach (QuestGiver questGiver in _questGivers)
                questGiver.QuestStatusChanged -= OnQuestStatusChanged;
        }

        public void Initialize()
        {
            InitializeQuestGivers();
        }

        public IEnumerable<Quest> GetQuests()
        {
            return _questGivers.Select(q => q.Quest);
        }

        private void InitializeQuestGivers()
        {
            Dictionary<string, QuestStatus> quests = _questsProvider.LoadQuests();
            foreach (QuestGiver questGiver in _questGivers)
            {
                var id = questGiver.QuestID;
                QuestStatus questStatus = quests.ContainsKey(id) ? quests[id] : new QuestStatus();

                questGiver.Initialize(questStatus);

                questGiver.QuestStatusChanged += OnQuestStatusChanged;
            }
        }

        private void OnQuestStatusChanged(string id, QuestStatus status)
        {
            _questsProvider.UpdateQuest(id, status);

            if (status.State == QuestState.Done)
            {
                _audioService.PlaySound(_questDoneSound);
            }
        }
    }
}