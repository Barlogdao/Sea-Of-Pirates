using Project.Systems.Quests;
using System.Collections.Generic;

namespace Project.Interfaces.Data
{
    public interface IQuestsProvider
    {
        Dictionary<string, QuestStatus> LoadQuests();
        void UpdateQuest(string id, QuestStatus status);
    }
}