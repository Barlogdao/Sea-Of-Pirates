using System;
using System.Collections.Generic;
using Project.SDK.Leaderboard;

namespace Project.Interfaces.SDK
{
    public interface ILeaderboardService
    {
        bool IsPlayerAuthorized { get; }
        void AuthorizePlayer();
        void LoadPlayers(Action<List<LeaderboardPlayer>> onLoadCallback);
        void SetPlayerScore(int score);
    }
}