using UnityEngine;
using TMPro;

public class CharacterStateController : MonoBehaviour
{
    public Animator animator;  // ���⪺ Animator
    public TextMeshProUGUI textBox;  // TextMeshPro �� UI ��r��
    public int charsPerLoop = 10; // �C X �Ӧr���� 1 ���ʵe

    private int remainingLoops = 0; // �Ѿl���񦸼�
    private string lastText = ""; // �O���W�@�����奻
    private bool isPlaying = false; // �T�O�ʵe����ɤ��|����Ĳ�o

    void Update()
    {
        // �p�G��r�o���ܤơA�B�ثe�S���b����ʵe
        if (textBox.text != lastText && !isPlaying)
        {
            lastText = textBox.text; // ��s�O��
            if (textBox.text.Length > 0)
            {
                StartTalking();
            }
        }
    }

    void StartTalking()
    {
        remainingLoops = Mathf.CeilToInt(textBox.text.Length / (float)charsPerLoop); // �ھڷs���e�p�⼽�񦸼�
        isPlaying = true; // �аO���b����ʵe

        animator.SetBool("isTalking", true);
        animator.SetBool("isIdle", false);

        StartCoroutine(PlayTalkingAnimation());
    }

    System.Collections.IEnumerator PlayTalkingAnimation()
    {
        while (remainingLoops > 0)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float animationDuration = stateInfo.length;

            yield return new WaitForSeconds(animationDuration); // ���ݰʵe����

            remainingLoops--;

            if (remainingLoops <= 0) // ���񵲧�
            {
                animator.SetBool("isTalking", false);
                animator.SetBool("isIdle", true);
                isPlaying = false; // �аO�ʵe���񧹲��A���\�����s�ܤ�
                yield break;
            }
        }
    }
}