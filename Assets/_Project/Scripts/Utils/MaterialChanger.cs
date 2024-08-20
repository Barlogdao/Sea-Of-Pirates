using NaughtyAttributes;
using UnityEngine;


public class MaterialChanger : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Material _material;

    [ExecuteInEditMode]
    private void ChangeMaterial(Transform transform)
    {
        if (transform.TryGetComponent<MeshRenderer>(out var renderer))
        {
            renderer.material = _material;
        }

        foreach (Transform child in transform)
        {
            ChangeMaterial(child);
        }
    }

    private void OnValidate()
    {
        if (_material == null || _transform == null)
            return;

        ChangeMaterial(_transform);
    }
}