using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SitOnChair : MonoBehaviour
{
    public Transform chairTarget;  // �Ȥl�����U��m
    public Transform standTarget;  // ���_�Ӫ���m (��ʫ��w)
    public GameObject xrRig;       // XR Rig (���a)
    public CharacterController characterController; // ����a����
    public XROrigin xrRigComponent; // XR Rig ���� (�Ω�p�Ⲿ��)

    private bool isSeated = false; // �l�ܪ��a�O�_���U
    private Vector2 moveInput; // ���a���ʿ�J

    void Start()
    {
        if (chairTarget == null || standTarget == null || xrRig == null) return;

        // ������� XR Rig �ե�
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

        // ���� XR Rig ��Ȥl��m
        xrRig.transform.position = chairTarget.position;
        xrRig.transform.rotation = chairTarget.rotation;

        // �ҥ� "�T�β���" �Ҧ�
        isSeated = true;
    }

    void StandUp()
    {
        if (standTarget == null || xrRig == null) return;

        // **���ʨ��ʫ��w�����ߦ�m**
        xrRig.transform.position = standTarget.position;
        xrRig.transform.rotation = standTarget.rotation;

        isSeated = false;
    }

    void Update()
    {
        if (isSeated)
        {
            // �T��ʡA�N `CharacterController` �����ʦV�q�]���s
            if (characterController != null)
            {
                characterController.Move(Vector3.zero);
            }
        }
    }
}
