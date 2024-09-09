using Project.Enemies.Logic.States.Idle;
using Project.Enemies.View;
using Project.Players.Logic;
using UnityEngine;

namespace Project.Enemies.Logic.States.Battle
{
    public abstract class BattleState : AliveState
    {
        private EnemyView _enemyView;

        protected Vector3 DirectionToPlayer => Player.Position - Enemy.transform.position;
        protected Vector3 RightSideDirection => Enemy.transform.right;
        protected Vector3 LeftSideDirection => -Enemy.transform.right;

        public override void Enter()
        {
            base.Enter();
            Detector.PlayerLost += OnPlayerLost;
            Player.Died += OnPlayerDied;
            _enemyView.ShowHud();
        }

        public override void Exit()
        {
            base.Exit();
            Detector.PlayerLost -= OnPlayerLost;
            Player.Died -= OnPlayerDied;
            _enemyView.HideHud();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _enemyView = Enemy.View;
        }

        protected BoardSide GetClosestSide()
        {
            float rightDot = Vector3.Dot(RightSideDirection, DirectionToPlayer);
            float leftDot = Vector3.Dot(LeftSideDirection, DirectionToPlayer);

            return rightDot >= leftDot ? BoardSide.Right : BoardSide.Left;
        }

        private void OnPlayerLost()
        {
            StateMachine.SetState<IdleState>();
        }

        private void OnPlayerDied()
        {
            StateMachine.SetState<IdleState>();
        }
    }
}
