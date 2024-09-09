using DG.Tweening;
using UnityEngine;

namespace Project.Enemies.View
{
    public class AttackRangeView : MonoBehaviour
    {
        private const float ScaleMultiplier = 2f;

        [SerializeField] private float _showDuration = 0.2f;
        [SerializeField] private Ease _showEase = Ease.InOutBack;

        private float _attackRange;
        private bool IsZeroAttackRange => _attackRange == 0f;

        private Vector3 _attackRangeScale;

        public void Initialize(float attackRange)
        {
            _attackRange = attackRange;

            if (IsZeroAttackRange == false)
            {
                _attackRangeScale = attackRange * ScaleMultiplier * Vector3.one;
                transform.localScale = _attackRangeScale;
            }

            HideAttackRange();
        }

        public void ShowAttackRange()
        {
            if (IsZeroAttackRange)
                return;

            transform.gameObject.SetActive(true);

            transform.localScale = Vector3.zero;
            transform.DOScale(_attackRangeScale, _showDuration)
                .SetEase(_showEase)
                .SetLink(gameObject);
        }

        public void HideAttackRange()
        {
            transform.gameObject.SetActive(false);
        }
    }
}