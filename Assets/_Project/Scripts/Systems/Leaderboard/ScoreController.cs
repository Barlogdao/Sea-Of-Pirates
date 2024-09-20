using System.Collections;
using Project.Interfaces.Enemies;
using Project.Interfaces.SDK;
using Project.Players.Logic;
using UnityEngine;
using Zenject;
using PlayerPrefs = UnityEngine.PlayerPrefs;

namespace Project.Controllers
{
    public class ScoreController : MonoBehaviour
    {
        private const string SaveNumberEnemysKilled = "SaveNumberEnemysKilled";

        private ILeaderboardService _leaderboardService;
        private PlayerAttack _playerAttack;
        private int _killsCount = 0;
        private Coroutine _updateLeaderboardCoroutine;
        private WaitForSeconds _delayTime = new(5f);

        private void OnDestroy()
        {
            if (_playerAttack != null)
            {
                _playerAttack.EnemyKilled -= OnEnemyKilled;
            }
        }

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
            yield return _delayTime;
            _leaderboardService.SetPlayerScore(_killsCount);
            PlayerPrefs.SetInt(SaveNumberEnemysKilled, _killsCount);
        }
    }
}