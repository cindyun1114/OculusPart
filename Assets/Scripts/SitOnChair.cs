using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

[RequireComponent(typeof(CharacterController))] // �T�O�� CharacterController
public class SitOnChair : MonoBehaviour
{
    [Header("���n�j�w")]
    [Tooltip("��J XR Origin ����")]
    public XROrigin xrRig;
    [Tooltip("��J Locomotion System �U�� Move ����")]
    public DynamicMoveProvider moveProvider;
    [Tooltip("���w�Ȥl���U��m")]
    public Transform chairTarget;

    [Header("�i���]�w")]
    [Tooltip("�_���ɩ��e���ʪ��Z��")]
    public float standForwardOffset = 0.3f;
    [Tooltip("���U�ɪ��I���鰪��")]
    public float seatedHeight = 1.0f;

    private CharacterController characterController;
    private bool isSeated = false;
    private float originalHeight;     // ��l�I���鰪��
    private Vector3 originalCenter;  // ��l�I���餤��

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // �x�s��l�Ѽ�
        originalHeight = characterController.height;
        originalCenter = characterController.center;

        // �۰ʸj�wĵ�i
        if (xrRig == null || moveProvider == null || chairTarget == null)
        {
            Debug.LogError("���n���󥼸j�w�I���ˬd Inspector �]�w");
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

        // �T�β��ʨt��
        moveProvider.enabled = false;

        // �վ�I���鬰���U���A
        characterController.height = seatedHeight;
        characterController.center = new Vector3(0, seatedHeight * 0.5f, 0); // �����I�m��

        // ���� Step Offset ���~
        float maxStep = characterController.height + 2 * characterController.radius;
        characterController.stepOffset = Mathf.Min(characterController.stepOffset, maxStep);

        // �ǰe��Ȥl��m�]�O�����������^
        Vector3 headOffset = xrRig.Camera.transform.position - xrRig.transform.position;
        headOffset.y = 0;
        xrRig.transform.position = chairTarget.position - headOffset;

        isSeated = true;
    }

    void StandUp()
    {
        if (!ValidateComponents()) return;

        // �ҥβ��ʨt��
        moveProvider.enabled = true;

        // ��_�I����Ѽ�
        characterController.height = originalHeight;
        characterController.center = originalCenter;

        // �p��_����m�]�Ȥl�e�� + �Y�������^
        Vector3 headOffset = xrRig.Camera.transform.position - xrRig.transform.position;
        headOffset.y = 0;
        Vector3 standPosition = chairTarget.position +
                              (chairTarget.forward * standForwardOffset) +
                              headOffset;

        xrRig.transform.position = standPosition;

        isSeated = false;
    }

    // �������Ҥ�k
    private bool ValidateComponents()
    {
        if (xrRig == null)
        {
            Debug.LogError("XR Rig ���j�w");
            return false;
        }
        if (moveProvider == null)
        {
            Debug.LogError("Move Provider ���j�w");
            return false;
        }
        if (chairTarget == null)
        {
            Debug.LogError("�Ȥl��m�����w");
            return false;
        }
        return true;
    }
}