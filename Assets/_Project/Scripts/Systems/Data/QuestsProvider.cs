using System.Linq;
using System.Collections.Generic;
using Project.Interfaces.Data;
using Project.Systems.Quests;

namespace Project.Systems.Data
{
    public class QuestsProvider : IQuestsProvider
    {
        private IQuestsData _questsData;
        private Dictionary<string, QuestStatus> _quests;

        public QuestsProvider(IQuestsData questsData)
        {
            _questsData = questsData;
            _quests = null;
        }

        public Dictionary<string, QuestStatus> LoadQuests()
        {
            if (_quests != null)
            {
                return _quests;
            }

            _quests = new Dictionary<string, QuestStatus>();

            foreach (QuestData questData in _questsData.Quests)
            {
                _quests.Add(questData.ID, new QuestStatus(questData.State, questData.Progress));
            }

            return _quests;
        }

        public void UpdateQuest(string questID, QuestStatus status)
        {
            _quests[questID] = status;

            if (_questsData.Quests.Any(q => q.ID == questID) == false)
            {
                _questsData.Quests.Add(new QuestData(questID, status));
            }

            QuestData data = _questsData.Quests.Find(q => q.ID == questID);
            data.Update(status);

            _questsData.Save();
        }
    }
}