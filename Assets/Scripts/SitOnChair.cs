using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SitOnChair : MonoBehaviour
{
    public Transform chairTarget;  // 椅子的坐下位置
    public Transform standTarget;  // 站起來的位置 (手動指定)
    public GameObject xrRig;       // XR Rig (玩家)
    public CharacterController characterController; // 控制玩家移動
    public XROrigin xrRigComponent; // XR Rig 控制 (用於計算移動)

    private bool isSeated = false; // 追蹤玩家是否坐下
    private Vector2 moveInput; // 玩家移動輸入

    void Start()
    {
        if (chairTarget == null || standTarget == null || xrRig == null) return;

        // 嘗試獲取 XR Rig 組件
        xrRigComponent = xrRig.GetComponent<XROrigin>();
    }

    public void ToggleSeat()
    {
        if (!isSeated)
        {
            SitDown();
        }
        else
        {
            StandUp();
        }
    }

    void SitDown()
    {
        if (chairTarget == null || xrRig == null) return;

        // 移動 XR Rig 到椅子位置
        xrRig.transform.position = chairTarget.position;
        xrRig.transform.rotation = chairTarget.rotation;

        // 啟用 "禁用移動" 模式
        isSeated = true;
    }

    void StandUp()
    {
        if (standTarget == null || xrRig == null) return;

        // **移動到手動指定的站立位置**
        xrRig.transform.position = standTarget.position;
        xrRig.transform.rotation = standTarget.rotation;

        isSeated = false;
    }

    void Update()
    {
        if (isSeated)
        {
            // 禁止移動，將 `CharacterController` 的移動向量設為零
            if (characterController != null)
            {
                characterController.Move(Vector3.zero);
            }
        }
    }
}
