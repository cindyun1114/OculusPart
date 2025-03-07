using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

[RequireComponent(typeof(CharacterController))] // 確保有 CharacterController
public class SitOnChair : MonoBehaviour
{
    [Header("必要綁定")]
    [Tooltip("拖入 XR Origin 物件")]
    public XROrigin xrRig;
    [Tooltip("拖入 Locomotion System 下的 Move 物件")]
    public DynamicMoveProvider moveProvider;
    [Tooltip("指定椅子坐下位置")]
    public Transform chairTarget;

    [Header("進階設定")]
    [Tooltip("起身時往前移動的距離")]
    public float standForwardOffset = 0.3f;
    [Tooltip("坐下時的碰撞體高度")]
    public float seatedHeight = 1.0f;

    private CharacterController characterController;
    private bool isSeated = false;
    private float originalHeight;     // 原始碰撞體高度
    private Vector3 originalCenter;  // 原始碰撞體中心

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // 儲存原始參數
        originalHeight = characterController.height;
        originalCenter = characterController.center;

        // 自動綁定警告
        if (xrRig == null || moveProvider == null || chairTarget == null)
        {
            Debug.LogError("重要元件未綁定！請檢查 Inspector 設定");
        }
    }

    public void ToggleSeat()
    {
        if (!isSeated) SitDown();
        else StandUp();
    }

    void SitDown()
    {
        if (!ValidateComponents()) return;

        // 禁用移動系統
        moveProvider.enabled = false;

        // 調整碰撞體為坐下狀態
        characterController.height = seatedHeight;
        characterController.center = new Vector3(0, seatedHeight * 0.5f, 0); // 中心點置中

        // 防止 Step Offset 錯誤
        float maxStep = characterController.height + 2 * characterController.radius;
        characterController.stepOffset = Mathf.Min(characterController.stepOffset, maxStep);

        // 傳送到椅子位置（保持水平偏移）
        Vector3 headOffset = xrRig.Camera.transform.position - xrRig.transform.position;
        headOffset.y = 0;
        xrRig.transform.position = chairTarget.position - headOffset;

        isSeated = true;
    }

    void StandUp()
    {
        if (!ValidateComponents()) return;

        // 啟用移動系統
        moveProvider.enabled = true;

        // 恢復碰撞體參數
        characterController.height = originalHeight;
        characterController.center = originalCenter;

        // 計算起身位置（椅子前方 + 頭部偏移）
        Vector3 headOffset = xrRig.Camera.transform.position - xrRig.transform.position;
        headOffset.y = 0;
        Vector3 standPosition = chairTarget.position +
                              (chairTarget.forward * standForwardOffset) +
                              headOffset;

        xrRig.transform.position = standPosition;

        isSeated = false;
    }

    // 元件驗證方法
    private bool ValidateComponents()
    {
        if (xrRig == null)
        {
            Debug.LogError("XR Rig 未綁定");
            return false;
        }
        if (moveProvider == null)
        {
            Debug.LogError("Move Provider 未綁定");
            return false;
        }
        if (chairTarget == null)
        {
            Debug.LogError("椅子位置未指定");
            return false;
        }
        return true;
    }
}