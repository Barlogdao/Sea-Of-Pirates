using Project.Players.Logic;
using UnityEngine;

namespace Project.Interactables
{
    public class ZoneHeal : InteractableZone
    {
        protected override void OnPlayerEntered(Player player)
        {
            base.OnPlayerEntered(player);

            player.Heal();
        }
    }
}