using UnityEngine;

public class PlayerPrefsReseter : MonoBehaviour
{
    // ��� ������ ������� � �������� ������� � ���������� ���� ���������� (3 ����� ������ - ������)
    [ContextMenu("Reset Prefs")]
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}