using Project.SDK.Leaderboard;
using Project.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Leaderboard
{
    public class LeaderboardWindow : UiWindow
    {
        [SerializeField] private LeaderboardView _leaderboardView;
        [SerializeField] private RectTransform _leaderboardPanel;
        [SerializeField] private RectTransform _authorizationPanel;
        [SerializeField] private Button _authorizationButton;

        public event Action AuthorizationRequested;

        protected override void Awake()
        {
            base.Awake();
            _authorizationButton.onClick.AddListener(OnAuthorizationClicked);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _authorizationButton.onClick.RemoveListener(OnAuthorizationClicked);
        }

        public void OpenLeaderboardPanel(List<LeaderboardPlayer> leaderboardPlayers)
        {
            _leaderboardView.ConstructLeaderboard(leaderboardPlayers);
            _leaderboardPanel.gameObject.SetActive(true);
            base.Show();
        }

        public void OpenAuthorizationPanel()
        {
            _authorizationPanel.gameObject.SetActive(true);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            _authorizationPanel.gameObject.SetActive(false);
            _leaderboardPanel.gameObject.SetActive(false);
        }

        private void OnAuthorizationClicked()
        {
            AuthorizationRequested?.Invoke();
            Hide();
        }
    }
}
