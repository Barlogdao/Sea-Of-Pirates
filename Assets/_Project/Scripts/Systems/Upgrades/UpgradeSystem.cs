using Project.UI.Upgrades;

namespace Project.Systems.Upgrades
{
    public class UpgradeSystem
    {
        private readonly UpgradeButton _upgradeButton;
        private readonly UpgradeWindow _upgradeWindow;

        public UpgradeSystem(UpgradeButton upgradeButton, UpgradeWindow upgradeWindow)
        {
            _upgradeButton = upgradeButton;
            _upgradeWindow = upgradeWindow;
        }

        public void Show()
        {
            _upgradeButton.Show(_upgradeWindow.Open);
        }

        public void Hide()
        {
            _upgradeButton.Hide();
            _upgradeWindow.Hide();
        }
    }
}