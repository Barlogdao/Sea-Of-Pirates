using System;
using System.Collections.Generic;
using Project.Interfaces.SDK;

namespace Project.SDK.Leaderboard
{
    public class MockLeaderboardService : ILeaderboardService
    {
        public bool IsPlayerAuthorized { get; private set; } = false; 

        public void AuthorizePlayer()
        {
            IsPlayerAuthorized = true;
        }

        public void LoadPlayers(Action<List<LeaderboardPlayer>> onLoadCallback)
        {
            List<LeaderboardPlayer> players = new()
            {
                new LeaderboardPlayer(1, "Barlogdao",100),
                new LeaderboardPlayer(2, "ArturTeterur", 300),
                new LeaderboardPlayer(3, "kriSasaki", 9999),
                new LeaderboardPlayer(4, null, 5),
                new LeaderboardPlayer(5, null, 4),
                new LeaderboardPlayer(6, null, 3),
                new LeaderboardPlayer(7, null, 2),
            };

            onLoadCallback( players );
        }

        public void SetPlayerScore(int score)
        {

        }
    }
}