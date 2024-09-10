using Project.Utils.Extensions;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CameraLook : MonoBehaviour
{
    private Canvas _canvas;
    private Camera _camera;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _camera = Camera.main;
        _canvas.worldCamera = _camera;
    }

    private void LateUpdate()
    {
        Quaternion rotation = _camera.transform.rotation;
        Vector3 rot = rotation.eulerAngles.WithX(transform.rotation.eulerAngles.x);
        transform.rotation = rotation;
    }
}
