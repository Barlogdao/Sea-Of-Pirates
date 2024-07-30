﻿using UnityEngine.UI;

namespace Project.UI.Upgrades
{
    public class UpgradeSystemView
    {
        private readonly Button _upgradeButton;
        private readonly UpgradeWindow _upgradeWindow;

        public UpgradeSystemView(Button upgradeButton, UpgradeWindow upgradeWindow)
        {
            _upgradeButton = upgradeButton;
            _upgradeWindow = upgradeWindow;

            HideButton();
        }

        public void Show()
        {
            _upgradeButton.gameObject.SetActive(true);
            _upgradeButton.onClick.AddListener(() => _upgradeWindow.Show());
        }

        public void Hide()
        {
            _upgradeWindow.Hide();
            HideButton();
        }

        private void HideButton()
        {
            _upgradeButton.onClick.RemoveAllListeners();
            _upgradeButton.gameObject.SetActive(false);
        }
    }
}