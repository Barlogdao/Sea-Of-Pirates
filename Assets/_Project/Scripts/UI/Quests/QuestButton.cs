using Project.Utils;
using Zenject;

namespace Project.UI.Quests
{
    public class QuestButton: UiButton
    {
        [Inject]
        private void Construct()
        {
            Hide();
        }
    }
}