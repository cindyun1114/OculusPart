using UnityEngine;

public class SitOnChair : MonoBehaviour
{
    public Transform chairTarget;  // �Ȥl�����U��m (ChairTarget)
    public GameObject xrRig;       // XR Rig (���a)
    public CharacterController characterController; // ����a����

    private Vector3 originalStandPosition; // ��l���ߦ�m
    private Quaternion originalStandRotation; // ��l���߱��ਤ��
    private bool isSeated = false; // �l�ܪ��a�O�_���U

    void Start()
    {
        if (chairTarget == null || xrRig == null) return;

        // **�p��T�w�����ߦ�m (�۹��Ȥl���e�� + ����)**
        originalStandPosition = chairTarget.position + chairTarget.forward * 0.5f + chairTarget.up * 0.3f;
        originalStandRotation = chairTarget.rotation; // �O���P�Ȥl�ۦP������
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

        // ���� XR Rig �� ChairTarget ��m
        xrRig.transform.position = chairTarget.position;
        xrRig.transform.rotation = chairTarget.rotation;

        // �T��a���ʡA�u�����Y
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        isSeated = true;
    }

    void StandUp()
    {
        if (xrRig == null) return;

        // **�T�w�ϥ� `originalStandPosition`�A�T�O�C�����_�Ӫ���m�@�P**
        xrRig.transform.position = originalStandPosition;
        xrRig.transform.rotation = originalStandRotation;

        // ���\���a����
        if (characterController != null)
        {
            characterController.enabled = true;
        }

        isSeated = false;
    }
}
