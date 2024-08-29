using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateBayPointer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform; // ������ �� ������ Player
    [SerializeField] private Camera _camera; // ������ �� ������, � ������� ���� �����
    [SerializeField] private Transform _pointerIconTransform; // ������ �� ������ ���������

    // ������� ������ ��� ��������� (� ��������� �� ������ � ������ ������)
    [SerializeField] private float _screenMarginX = 0.1f; // �������������� �������
    [SerializeField] private float _screenMarginY = 0.1f; // ������������ �������

    private Quaternion[] rotations = new Quaternion[]
    {
    Quaternion.Euler(0f, 0f, 90f),    // �����
    Quaternion.Euler(0f, 0f, -90f),   // ����
    Quaternion.Euler(0f, 0f, 180f),   // �����
    Quaternion.Euler(0f, 0f, 0f),     // ������
    Quaternion.identity                // �� ��������� (� ������ ������������������ �������)
    };

    private void Update()
    {
        Vector3 formPlayerToEnemy = transform.position - _playerTransform.position;
        Ray ray = new Ray(_playerTransform.position, formPlayerToEnemy);

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        float minDistance = Mathf.Infinity;
        int planeIndex = 0;

        for (int i = 0; i < 6; i++)
        {
            if (planes[i].Raycast(ray, out float distance))
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                    planeIndex = i;
                }
            }
        }

        minDistance = Mathf.Clamp(minDistance, 0.0f, formPlayerToEnemy.magnitude);
        Vector3 worldPosition = ray.GetPoint(minDistance);
        Vector3 screenPosition = _camera.WorldToScreenPoint(worldPosition);

        // ������������ ��������� � �������� �������� ������ ������
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // ��������� ����� ���������� � ������ ��������
        screenPosition.x = Mathf.Clamp(screenPosition.x, screenWidth * _screenMarginX, screenWidth * (1 - _screenMarginX));
        screenPosition.y = Mathf.Clamp(screenPosition.y, screenHeight * _screenMarginY, screenHeight * (1 - _screenMarginY));

        _pointerIconTransform.position = screenPosition;
        _pointerIconTransform.rotation = GetIconRotation(planeIndex);
    }

    // ����� ��� ��������� ����������� ������� ��������� � ����������� �� ���������
    Quaternion GetIconRotation(int planeIndex)
    {
        if (planeIndex < 0 || planeIndex >= rotations.Length)
        {
            return rotations[4]; // ���������� �������� �� ���������, ���� ������ ������� �� �������
        }

        return rotations[planeIndex];
    }
}
