using UnityEngine;

namespace Project.Systems.Interactables
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class InteractableZone : MonoBehaviour
    {
        [SerializeField] private float _triggerZoneRadius = 30f;
        [SerializeField] private Color _gizmosColor = Color.white;

        private SphereCollider _triggerZone;

        private void Awake()
        {
            _triggerZone = GetComponent<SphereCollider>();
            _triggerZone.isTrigger = true;
            _triggerZone.radius = _triggerZoneRadius;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireSphere(transform.position, _triggerZoneRadius);
        }
    }
}