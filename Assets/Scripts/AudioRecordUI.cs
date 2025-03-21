//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class AudioRecordUI : MonoBehaviour
//{
//    [SerializeField] private Button recordButton;    // 录音按钮
//    private TMP_Text buttonText;                     // 按钮文字
//    private Image buttonImage;                       // 按钮图片组件
//    private AudioManager audioManager;
//    private bool isRecording = false;

//    // 按钮颜色设置
//    private Color normalColor = Color.white;         // 普通状态颜色
//    private Color recordingColor = Color.red;        // 录音状态颜色

//    [System.Obsolete]
//    void Start()
//    {
//        // 获取AudioManager
//        audioManager = FindObjectOfType<AudioManager>();
//        if (audioManager == null)
//        {
//            Debug.LogError("找不到AudioManager！請確保場景存在AudioManager物件。");
//            return;
//        }

//        // 获取按钮组件
//        if (recordButton == null)
//        {
//            Debug.LogError("未設置錄音按鈕！請在Inspector中設置。");
//            return;
//        }

//        // 获取按钮文字和图片组件
//        buttonText = recordButton.GetComponentInChildren<TMP_Text>();
//        buttonImage = recordButton.GetComponent<Image>();

//        // 设置初始状态
//        buttonText.text = "開始錄音";
//        buttonImage.color = normalColor;

//        // 添加点击事件
//        recordButton.onClick.AddListener(OnRecordButtonClick);
//    }

//    void OnRecordButtonClick()
//    {
//        if (!isRecording)
//        {
//            // 开始录音
//            audioManager.StartRecording();
//            buttonText.text = "停止錄音";
//            buttonImage.color = recordingColor;
//        }
//        else
//        {
//            // 停止录音
//            audioManager.StopRecording();
//            buttonText.text = "開始錄音";
//            buttonImage.color = normalColor;
//        }
//        isRecording = !isRecording;
//    }

//    // 当脚本被禁用时确保重置状态
//    void OnDisable()
//    {
//        if (isRecording)
//        {
//            audioManager.StopRecording();
//            buttonText.text = "開始錄音";
//            buttonImage.color = normalColor;
//            isRecording = false;
//        }
//    }
//}