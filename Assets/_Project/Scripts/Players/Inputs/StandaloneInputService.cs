using UnityEngine;

namespace Project.Players.Inputs
{
    public class StandaloneInputService : InputService
    {
        private Vector2 _currentAxis = Vector2.zero;

        public override Vector2 Axis
        {
            get
            {
                Vector2 targetAxis = SimpleInputAxis();

                if (targetAxis == Vector2.zero)
                {
                    targetAxis = UnityAxis();
                }

                return SmoothTransition(targetAxis);
            }
        }

        private static Vector2 UnityAxis() =>
            new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));

        private Vector2 SmoothTransition(Vector2 targetAxis)
        {
            float smoothTime = 0.1f;
            _currentAxis = Vector2.Lerp(_currentAxis, targetAxis, Time.deltaTime / smoothTime);
            return _currentAxis;
        }
    }
}