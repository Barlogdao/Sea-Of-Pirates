using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCostView : MonoBehaviour
{
    [SerializeField] private Image _ResourceIcon;
    [SerializeField] private TMP_Text _amount;

    private void Awake()
    {
        Hide();
    }

    public void UpdateView(Sprite resourceSprite, string amount)
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