namespace Project.UI.Upgrades
{
    public class UpgradeSystemView
    {
        private readonly UpgradeButton _upgradeButton;
        private readonly UpgradeWindow _upgradeWindow;

        public UpgradeSystemView(UpgradeButton upgradeButton, UpgradeWindow upgradeWindow)
        {
            _upgradeButton = upgradeButton;
            _upgradeWindow = upgradeWindow;
        }

        public void Show()
        {
            _upgradeButton.Show(_upgradeWindow.Show);
        }

        public void Hide()
        {
            _upgradeButton.Hide();
            _upgradeWindow.Hide();
        }
    }
}