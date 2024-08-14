using Cysharp.Threading.Tasks;
using Project.Utils;
using Project.Utils.Extensions;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.Reward
{
    [RequireComponent(typeof(RectTransform))]
    public class RewardView : MonoBehaviour
    {
        private const float ZeroAlpha = 0f;
        private const float MaxAlpha = 1f;

        [SerializeField] private Button _button;
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private TMP_Text _rewardAmount;
        [SerializeField] private Slider _timeSlider;
        [SerializeField] private TMP_Text _timerlabel;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private AppearingUITween _tween;

        private Coroutine _offerRoutine;

        public event Action OfferExpired;

        private void Awake()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            _tween.Initialize(rectTransform);
            _canvasGroup.alpha = ZeroAlpha;
        }

        public void Show(Sprite rewardSprite, int rewardAmount, float offerDuration, Action rewardCallback)
        {

            _rewardIcon.sprite = rewardSprite;
            _rewardAmount.text = rewardAmount.ToNumericalString();

            _button.interactable = true;
            _button.onClick.AddListener(() => RequestReward(rewardCallback));

            _canvasGroup.alpha = MaxAlpha;
            _tween.Appear();

            _offerRoutine = StartCoroutine(RewardOffering(offerDuration));
        }

        public async UniTaskVoid Hide()
        {
            if (_offerRoutine != null)
            {
                StopCoroutine(_offerRoutine);
                _offerRoutine = null;
            }

            _button.interactable = false;
            _button.onClick.RemoveAllListeners();

            OfferExpired.Invoke();

            await _tween.DissapearAsync(destroyCancellationToken);

            _canvasGroup.alpha = ZeroAlpha;
        }

        private void UpdateTimeBar(float offerDuration, float elapsedTime)
        {
            _timeSlider.value = _timeSlider.maxValue - elapsedTime / offerDuration;

            TimeSpan timeLeft = TimeSpan.FromSeconds(offerDuration - elapsedTime);
            _timerlabel.text = timeLeft.ToString(@"mm\:ss");
        }

        private IEnumerator RewardOffering(float offerDuration)
        {
            float elapsedTime = 0f;

            yield return null;

            while (elapsedTime < offerDuration)
            {
                elapsedTime += Time.deltaTime;
                UpdateTimeBar(offerDuration, elapsedTime);
                yield return null;
            }

            Hide().Forget();
        }

        private void RequestReward(Action rewardCallback)
        {
            rewardCallback();
            Hide().Forget();
        }
    }
}