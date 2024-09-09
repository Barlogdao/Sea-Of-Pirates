using Project.Systems.Quests;

namespace Project.Systems.Data
{
    [System.Serializable]
    public class QuestData
    {
        public string ID;
        public QuestState State = QuestState.Avaliable;
        public int Progress;

        public QuestData(string id, QuestStatus status)
        {
            ID = id;
            State = status.State;
            Progress = status.Progress;
        }

        public void Update(QuestStatus status)
        {
            State = status.State;
            Progress = status.Progress;
        }
    }
}