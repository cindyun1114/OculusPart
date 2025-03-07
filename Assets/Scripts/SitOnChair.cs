using UnityEngine;

public class SitOnChair : MonoBehaviour
{
    public Transform chairTarget;  // 椅子的坐下位置 (ChairTarget)
    public GameObject xrRig;       // XR Rig (玩家)
    public CharacterController characterController; // 控制玩家移動

    private Vector3 originalStandPosition; // 初始站立位置
    private Quaternion originalStandRotation; // 初始站立旋轉角度
    private bool isSeated = false; // 追蹤玩家是否坐下

    void Start()
    {
        if (chairTarget == null || xrRig == null) return;

        // **計算固定的站立位置 (相對於椅子的前方 + 高度)**
        originalStandPosition = chairTarget.position + chairTarget.forward * 0.5f + chairTarget.up * 0.3f;
        originalStandRotation = chairTarget.rotation; // 保持與椅子相同的旋轉
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

        // 移動 XR Rig 到 ChairTarget 位置
        xrRig.transform.position = chairTarget.position;
        xrRig.transform.rotation = chairTarget.rotation;

        // 禁止玩家移動，只能轉頭
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        isSeated = true;
    }

    void StandUp()
    {
        if (xrRig == null) return;

        // **固定使用 `originalStandPosition`，確保每次站起來的位置一致**
        xrRig.transform.position = originalStandPosition;
        xrRig.transform.rotation = originalStandRotation;

        // 允許玩家移動
        if (characterController != null)
        {
            characterController.enabled = true;
        }

        isSeated = false;
    }
}
