using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Upgrades
{
    public class UpgradeCostView : MonoBehaviour
    {
        [SerializeField] private Image _ResourceIcon;
        [SerializeField] private TMP_Text _amount;

        public void Set(Sprite resourceSprite, string amount)
        {
            gameObject.SetActive(true);
            _ResourceIcon.sprite = resourceSprite;
            _amount.text = amount;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}