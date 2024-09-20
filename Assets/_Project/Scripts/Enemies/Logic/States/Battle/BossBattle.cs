using Ami.BroAudio;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Project.Enemies.View;
using Project.Players.Logic;
using Project.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Project.Enemies.Logic.States.Battle
{
    [CreateAssetMenu(fileName = "BossBattle", menuName = "Configs/Enemies/States/BossBattle")]

    public class BossBattle : BattleState
    {
        [SerializeField] private float _attackAngle = 50;
        [SerializeField] float _attackPhaseDuration = 10f;
        [SerializeField] float _phaseSwitchDuration = 2f;
        [HorizontalLine(3f, EColor.Blue)]
        [SerializeField] float _volleyPhaseDuration = 10f;
        [SerializeField] float _volleyRange = 15f;
        [SerializeField] float _volleyInterval= 2f;
        [HorizontalLine(3f, EColor.Blue)]
        [SerializeField] float _projectileRadius = 4f;
        [SerializeField] float _projectileExplodeDelay = 4f;
        [SerializeField] int _projectilesAmount = 10;
        [SerializeField] private SoundID _shootSound;

        private readonly float _launchDelay = 0.05f;

        private float HalfAttackAngle => _attackAngle / 2f;

        public override void Enter()
        {
            base.Enter();
            EnterAttackPhase().Forget();
        }

        public override void Exit()
        {
            base.Exit();

            Enemy.AttackRangeView.HideAttackRange();
        }

        private async UniTaskVoid EnterAttackPhase()
        {
            Enemy.AttackRangeView.ShowAttackRange();

            await UniTask.Delay(TimeSpan.FromSeconds(_phaseSwitchDuration), cancellationToken: ExitToken);


            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ExitToken);

            UniTask phaseEndTask = UniTask.Delay(TimeSpan.FromSeconds(_attackPhaseDuration), cancellationToken: linkedCts.Token);
            UniTask PlayerInZoneTask = UniTask.WaitUntil(() => CanAttackPlayer(), cancellationToken: linkedCts.Token);

            UniTask<int>.Awaiter conditonAwaiter = UniTask.WhenAny(phaseEndTask, PlayerInZoneTask).GetAwaiter();

            while (conditonAwaiter.IsCompleted == false)
            {
                Enemy.Mover.RotateTowards(Player.Position, GetClosestSide());
                await UniTask.NextFrame();
            }

            linkedCts.Cancel();

            if (CanAttackPlayer())
            {
                Enemy.DealDamageAsync(Player).Forget();
            }

            EnterVolleyPhase().Forget();
        }

        private async UniTaskVoid EnterVolleyPhase()
        {
            Enemy.AttackRangeView.HideAttackRange();

            await UniTask.Delay(TimeSpan.FromSeconds(_phaseSwitchDuration), cancellationToken: ExitToken);

            UniTask.Awaiter phaseEndAwaiter = UniTask.Delay(TimeSpan.FromSeconds(_volleyPhaseDuration), cancellationToken: ExitToken).GetAwaiter();

            while (!phaseEndAwaiter.IsCompleted)
            {
                LaunchProjectiles().Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(_volleyInterval), cancellationToken: ExitToken);
            }

            EnterAttackPhase().Forget();
        }

        private async UniTaskVoid LaunchProjectiles()
        {
            List<Vector3> projectilePositions = GetProjectilePositions();

            LayerMask playerMask = 1 << Player.PhysicsLayer;

            foreach (Vector3 position in projectilePositions)
            {
                Projectile projectile = Enemy.VfxSpawner.SpawnProjectile(position);

                projectile.ShootAsync(
                    _projectileRadius,
                    _projectileExplodeDelay,
                    playerMask,
                    () => Player.TakeDamage(Enemy.Damage)).Forget();

                AudioService.PlaySound(_shootSound);

                await UniTask.Delay(TimeSpan.FromSeconds(_launchDelay), cancellationToken: ExitToken);
            }
        }

        private List<Vector3> GetProjectilePositions()
        {
            List<Vector3> bombPositions = new List<Vector3>(_projectilesAmount);

            for (int i = 0; i < _projectilesAmount; i++)
            {
                Vector3 position = (UnityEngine.Random.insideUnitSphere * _volleyRange + Player.Position).WithZeroY();
                bombPositions.Add(position);
            }

            return bombPositions;
        }

        private bool CanAttackPlayer()
        {
            return IsPlayerInAttackRange() && IsPlayerInAttackZone();
        }

        private bool IsPlayerInAttackZone()
        {
            float rightAngle = Vector3.Angle(RightSideDirection, DirectionToPlayer);
            float leftAngle = Vector3.Angle(LeftSideDirection, DirectionToPlayer);

            return rightAngle <= HalfAttackAngle || leftAngle <= HalfAttackAngle;
        }

        private bool IsPlayerInAttackRange()
        {
            float distance = Vector3.Distance(Player.Position, Enemy.transform.position);

            return distance <= Config.AttackRange;
        }
    }
}
