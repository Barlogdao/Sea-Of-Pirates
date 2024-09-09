using Project.Enemies.View;
using Project.Players.Logic;
using Unity.Profiling;
using UnityEngine;

namespace Project.Enemies.Logic.States.Battle
{
    [CreateAssetMenu(fileName = "SimpleAttack", menuName = "Configs/Enemies/States/SimpleAttack")]
    public class AttackBattle : BattleState
    {
        [SerializeField] private float _attackAngle = 60;

        private AttackRangeView _attackRangeView;
        private float _timeToAttack;

        private float HalfAttackAngle => _attackAngle / 2f;

        public override void Enter()
        {
            base.Enter();
            ResetCooldown();
            _attackRangeView.ShowAttackRange();
        }

        public override void Exit()
        {
            base.Exit();
            _attackRangeView.HideAttackRange();
        }

        public override void Update()
        {
            base.Update();

            Enemy.Mover.RotateTowards(Player.Position, GetClosestSide());

            _timeToAttack -= Time.deltaTime;

            if (CanAttackPlayer())
            {
                Shoot();
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _attackRangeView = Enemy.AttackRangeView;
        }

        private void Shoot()
        {
            Enemy.DealDamageAsync(Player).Forget();
            ResetCooldown();
        }

        private void ResetCooldown()
        {
            _timeToAttack = Config.AttackCooldown;
        }

        private bool CanAttackPlayer()
        {
            if (Player.IsAlive == false)
                return false;

            return IsCannonsLoaded() && IsPlayerInAttackRange() && IsPlayerInAttackZone();
        }

        private bool IsPlayerInAttackRange()
        {
            float distance = Vector3.Distance(Player.Position, Enemy.transform.position);

            return distance <= Config.AttackRange;
        }

        private bool IsCannonsLoaded()
        {
            return _timeToAttack <= 0;
        }

        private bool IsPlayerInAttackZone()
        {
            float rightAngle = Vector3.Angle(RightSideDirection, DirectionToPlayer);
            float leftAngle = Vector3.Angle(LeftSideDirection, DirectionToPlayer);

            return rightAngle <= HalfAttackAngle || leftAngle <= HalfAttackAngle;
        }
    }
}