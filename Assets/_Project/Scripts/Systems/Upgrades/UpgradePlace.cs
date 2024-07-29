using Project.Players.PlayerLogic;
using Project.Systems.Interactables;
using Project.UI.Upgrades;
using UnityEngine;
using Zenject;

namespace Project.Systems.Upgrades
{
    [RequireComponent(typeof(SphereCollider))]
    public class UpgradePlace : InteractableZone
    {
        private UpgradeSystemView _upgradeSystemView;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _upgradeSystemView.Show();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _upgradeSystemView.Hide();
            }
        }

        [Inject]
        public void Construct(UpgradeSystemView upgradeSystemView)
        {
           _upgradeSystemView = upgradeSystemView;
        }
    }
}