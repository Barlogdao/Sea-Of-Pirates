using Project.SDK.Leaderboard;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI.Leaderboard
{
    public class LeaderboardView : MonoBehaviour
    {
        private const string AnonymousName = "Anonymous";

        [SerializeField] private Transform _container;
        [SerializeField] private LeaderboardElement _leaderboardElementPrefab;

        private List<LeaderboardElement> _spawnedElements = new();

        public void ConstructLeaderboard(List<LeaderboardPlayer> leaderboardPlayers)
        {
            ClearLeaderboard();

            foreach (LeaderboardPlayer player in leaderboardPlayers)
            {
                string playerName = player.Name;

                if (string.IsNullOrEmpty(playerName))
                    playerName = Lean.Localization.LeanLocalization.GetTranslationText(AnonymousName);

                LeaderboardElement leaderboardElementInstance = Instantiate(_leaderboardElementPrefab, _container);
                leaderboardElementInstance.Initialize(playerName, player.Rank, player.Score);

                _spawnedElements.Add(leaderboardElementInstance);
            }
        }

        private void ClearLeaderboard()
        {
            foreach (var element in _spawnedElements)
            {
                Destroy(element.gameObject);
            }

            _spawnedElements = new List<LeaderboardElement>();
        }
    }
}