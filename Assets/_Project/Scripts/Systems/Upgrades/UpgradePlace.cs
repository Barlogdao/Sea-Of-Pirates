using Project.Players.PlayerLogic;
using Project.Systems.Interactables;
using UnityEngine;
using Zenject;

namespace Project.Systems.Upgrades
{
    [RequireComponent(typeof(SphereCollider))]
    public class UpgradePlace : InteractableZone
    {
        private UpgradeSystem _upgradeSystem;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _upgradeSystem.Show();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _upgradeSystem.Hide();
            }
        }

        [Inject]
        private void Construct(UpgradeSystem upgradeSystemView)
        {
           _upgradeSystem = upgradeSystemView;
        }
    }
}