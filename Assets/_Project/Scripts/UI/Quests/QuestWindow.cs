using Project.Interfaces.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Quests
{
    public class QuestWindow : MonoBehaviour
    {
        [SerializeField] private Canvas _window;
        [SerializeField] private TMP_Text _decription;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TMP_Text _rewardAmount;
        [SerializeField] private TMP_Text _progressValue;
        [SerializeField] private Button _confirmButton;

        private void Awake()
        {
            Hide();
        }

        public void Show(IQuest quest)
        {
            _confirmButton.onClick.AddListener(() => OnButtonClicked(quest));

            _decription.text = quest.Config.Description;
            _rewardAmount.text = quest.Config.Reward.Amount.ToString();
            _rewardImage.sprite = quest.Config.Reward.Resource.Sprite;

            int currentProgress = quest.Status.Progress;
            int targetProgress = quest.Config.TargetAmount;
            string progress = $"{currentProgress}/{targetProgress}";
            _progressValue.text = progress;

            _window.enabled = true;
        }

        public void Hide()
        {
            _confirmButton.onClick.RemoveAllListeners();

            _window.enabled = false;
        }

        private void OnButtonClicked(IQuest quest)
        {
            quest.UpdateState();
            Hide();
        }
    }
}