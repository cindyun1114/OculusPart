using UnityEngine;
using UnityEngine.InputSystem; // �T�O�ϥ�Input System
using UnityEngine.UI;

public class LongPressBHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference longPressBAction; // �bInspector�s��"Long Press B"�ƥ�
    [SerializeField] private GameObject uiPage; // �ݭn���/���ê�UI����

    private void OnEnable()
    {
        if (longPressBAction != null)
            longPressBAction.action.performed += OnLongPressB;
    }

    private void OnDisable()
    {
        if (longPressBAction != null)
            longPressBAction.action.performed -= OnLongPressB;
    }

    private void OnLongPressB(InputAction.CallbackContext context)
    {
        if (uiPage != null)
        {
            uiPage.SetActive(!uiPage.activeSelf); // ����UI��ܪ��A
        }
    }
}
