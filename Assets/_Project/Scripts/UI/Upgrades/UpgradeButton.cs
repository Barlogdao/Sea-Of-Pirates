using Project.Utils;
using Zenject;

namespace Project.UI.Upgrades
{
    public class UpgradeButton : UiButton
    {
        [Inject]
        private void Construct()
        {
            Hide();
        }
    }
}