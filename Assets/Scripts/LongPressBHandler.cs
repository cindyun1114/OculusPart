using UnityEngine;
using UnityEngine.InputSystem; // 確保使用Input System
using UnityEngine.UI;

public class LongPressBHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference longPressBAction; // 在Inspector連結"Long Press B"事件
    [SerializeField] private GameObject uiPage; // 需要顯示/隱藏的UI頁面

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
            uiPage.SetActive(!uiPage.activeSelf); // 切換UI顯示狀態
        }
    }
}
