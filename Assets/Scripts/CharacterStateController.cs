using UnityEngine;
using TMPro;

public class CharacterStateController : MonoBehaviour
{
    public Animator animator;  // 角色的 Animator
    public TextMeshProUGUI textBox;  // TextMeshPro 的 UI 文字框
    public int charsPerLoop = 10; // 每 X 個字播放 1 次動畫

    private int remainingLoops = 0; // 剩餘播放次數
    private string lastText = ""; // 記錄上一次的文本
    private bool isPlaying = false; // 確保動畫執行時不會重複觸發

    void Update()
    {
        // 如果文字發生變化，且目前沒有在播放動畫
        if (textBox.text != lastText && !isPlaying)
        {
            lastText = textBox.text; // 更新記錄
            if (textBox.text.Length > 0)
            {
                StartTalking();
            }
        }
    }

    void StartTalking()
    {
        remainingLoops = Mathf.CeilToInt(textBox.text.Length / (float)charsPerLoop); // 根據新內容計算播放次數
        isPlaying = true; // 標記正在播放動畫

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

            yield return new WaitForSeconds(animationDuration); // 等待動畫播完

            remainingLoops--;

            if (remainingLoops <= 0) // 播放結束
            {
                animator.SetBool("isTalking", false);
                animator.SetBool("isIdle", true);
                isPlaying = false; // 標記動畫播放完畢，允許偵測新變化
                yield break;
            }
        }
    }
}