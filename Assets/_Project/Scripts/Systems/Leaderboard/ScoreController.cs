using System;
using System.Collections;
using Agava.YandexGames.Utility;
using Project.Interfaces.Enemies;
using Project.Interfaces.SDK;
using Project.Players.Logic;
using UnityEngine;
using Zenject;
using PlayerPrefs = UnityEngine.PlayerPrefs;

namespace Project.Controllers
{
    public class ScoreController : MonoBehaviour, IDisposable
    {
        private const string SaveNumberEnemysKilled = "SaveNumberEnemysKilled";

        private ILeaderboardService _leaderboardService;
        private PlayerAttack _playerAttack;
        private int _killsCount = 0;
        private Coroutine _updateLeaderboardCoroutine;
        private float _delayTime = 5f;

        [Inject]
        private void Construct(ILeaderboardService leaderboardService, PlayerAttack playerAttack)
        {
            _leaderboardService = leaderboardService;
            _playerAttack = playerAttack;
            _playerAttack.EnemyKilled += OnEnemyKilled;

            if (PlayerPrefs.HasKey(SaveNumberEnemysKilled))
            {
                _killsCount = PlayerPrefs.GetInt(SaveNumberEnemysKilled);
            }

            _updateLeaderboardCoroutine = null;
        }

        private void OnEnemyKilled(IEnemy enemy)
        {
            _killsCount++;
            UpdateLeaderboardScoreDelayed();
        }

        private void UpdateLeaderboardScoreDelayed()
        {
            if (_updateLeaderboardCoroutine != null)
            {
                StopCoroutine(_updateLeaderboardCoroutine);
            }

            _updateLeaderboardCoroutine = StartCoroutine(UpdateLeaderboardScoreAfterDelay());
        }

        private IEnumerator UpdateLeaderboardScoreAfterDelay()
        {
            yield return new WaitForSeconds(_delayTime);
            _leaderboardService.SetPlayerScore(_killsCount);
            PlayerPrefs.SetInt(SaveNumberEnemysKilled, _killsCount);
        }

        public void Dispose()
        {
            if (_playerAttack != null)
            {
                _playerAttack.EnemyKilled -= OnEnemyKilled;
            }
        }
    }
}