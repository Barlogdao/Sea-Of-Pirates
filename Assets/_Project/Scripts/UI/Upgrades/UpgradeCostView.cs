using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Upgrades
{
    public class UpgradeCostView : MonoBehaviour
    {
        [SerializeField] private Image _resourceIcon;
        [SerializeField] private TMP_Text _amount;
        [SerializeField] private Color _avaliableColor = Color.white;
        [SerializeField] private Color _unavaliableColor = Color.red;

        public void Set(Sprite resourceSprite, string amount, bool canSpend)
        {
            gameObject.SetActive(true);
            _resourceIcon.sprite = resourceSprite;
            _amount.text = amount;
            _amount.color = canSpend? _avaliableColor : _unavaliableColor;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}