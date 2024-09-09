using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Infrastructure
{
    public class StartLevelHandler : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _openingCamera;
        [SerializeField] private CinemachineBrain _brain;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _canvasJoystick;
        [SerializeField] private GameObject _canvasPonter;

        private async UniTaskVoid Start()
        {
            _canvasGroup.alpha = 0f;
            _canvasJoystick.SetActive(false);
            _canvasPonter.SetActive(false);
            await UniTask.Delay(2000);
            _openingCamera.gameObject.SetActive(false);
            await UniTask.WaitUntil(()=>_brain.IsBlending == false);
            _canvasGroup.alpha = 1f;
            _canvasJoystick.SetActive(true);
            _canvasPonter.SetActive(true);
        }
    }
}