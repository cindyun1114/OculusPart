using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private string serverUrl = "https://feynman-server.onrender.com";
    private bool isRecording = false;
    private AudioClip recordedClip;

    // 引用chatWithProfessor组件
    private chatWithProfessor chatManager;

    void Start()
    {
        // 通过Environment对象获取chatWithProfessor组件
        GameObject environment = GameObject.Find("Environment");
        if (environment != null)
        {
            chatManager = environment.GetComponent<chatWithProfessor>();
            if (chatManager == null)
            {
                Debug.LogError("Environment上找不到chatWithProfessor組件！");
            }
        }
        else
        {
            Debug.LogError("場景中找不到Environment對象！");
        }
    }

    // 开始录音
    public void StartRecording()
    {
        if (!isRecording)
        {
            if (Microphone.devices.Length == 0)
            {
                Debug.LogError("沒有找到麥克風設備！");
                return;
            }

            isRecording = true;
            string deviceName = Microphone.devices[0];
            recordedClip = Microphone.Start(deviceName, false, 60, 44100);
            Debug.Log($"開始錄音... 使用設備: {deviceName}");
        }
    }

    // 停止录音并发送
    public void StopRecording()
    {
        if (isRecording)
        {
            Microphone.End(null);
            isRecording = false;
            Debug.Log("錄音結束");
            StartCoroutine(ProcessAudio());
        }
    }

    private IEnumerator ProcessAudio()
    {
        // 将AudioClip转换为WAV格式
        byte[] wavData = AudioToWav(recordedClip);

        // 首先发送到转写API
        WWWForm form = new WWWForm();
        form.AddBinaryData("audio", wavData, "audio.wav", "audio/wav");

        using (UnityWebRequest www = UnityWebRequest.Post($"{serverUrl}/transcribe", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // 解析转写结果
                TranscriptionResponse response = JsonUtility.FromJson<TranscriptionResponse>(www.downloadHandler.text);
                Debug.Log($"轉寫文本: {response.text}");

                // 使用chatWithProfessor的SendMessage方法发送消息
                if (chatManager != null)
                {
                    chatManager.testInput.text = response.text;
                    StartCoroutine(chatManager.SendMessage());
                }
                else
                {
                    Debug.LogError("chatManager未初始化！");
                }
            }
            else
            {
                Debug.LogError($"轉寫錯誤: {www.error}");
            }
        }
    }

    // 将AudioClip转换为WAV格式的字节数组
    private byte[] AudioToWav(AudioClip clip)
    {
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (Int16)(samples[i] * 32767);
        }

        // 計算檔案大小
        int headerSize = 44;
        int dataSize = intData.Length * 2;  // 16-bit = 2 bytes per sample
        int fileSize = headerSize + dataSize;

        byte[] header = new byte[headerSize];

        // RIFF 標識
        header[0] = (byte)'R'; header[1] = (byte)'I'; header[2] = (byte)'F'; header[3] = (byte)'F';

        // 檔案大小
        header[4] = (byte)(fileSize & 0xFF);
        header[5] = (byte)((fileSize >> 8) & 0xFF);
        header[6] = (byte)((fileSize >> 16) & 0xFF);
        header[7] = (byte)((fileSize >> 24) & 0xFF);

        // WAVE 標識
        header[8] = (byte)'W'; header[9] = (byte)'A'; header[10] = (byte)'V'; header[11] = (byte)'E';

        // fmt 子塊
        header[12] = (byte)'f'; header[13] = (byte)'m'; header[14] = (byte)'t'; header[15] = (byte)' ';
        header[16] = 16; // 子塊大小
        header[17] = 0; header[18] = 0; header[19] = 0;

        // 音頻格式 (PCM)
        header[20] = 1; // PCM = 1
        header[21] = 0;

        // 聲道數
        header[22] = (byte)clip.channels;
        header[23] = 0;

        // 採樣率 (44.1kHz)
        int sampleRate = 44100;
        header[24] = (byte)(sampleRate & 0xFF);
        header[25] = (byte)((sampleRate >> 8) & 0xFF);
        header[26] = (byte)((sampleRate >> 16) & 0xFF);
        header[27] = (byte)((sampleRate >> 24) & 0xFF);

        // 位元率 (採樣率 * 聲道數 * 2)
        int byteRate = sampleRate * clip.channels * 2;
        header[28] = (byte)(byteRate & 0xFF);
        header[29] = (byte)((byteRate >> 8) & 0xFF);
        header[30] = (byte)((byteRate >> 16) & 0xFF);
        header[31] = (byte)((byteRate >> 24) & 0xFF);

        // 區塊對齊 (聲道數 * 2)
        header[32] = (byte)(clip.channels * 2);
        header[33] = 0;

        // 位元深度
        header[34] = 16; // 16 bits
        header[35] = 0;

        // data 子塊
        header[36] = (byte)'d'; header[37] = (byte)'a'; header[38] = (byte)'t'; header[39] = (byte)'a';

        // 數據大小
        header[40] = (byte)(dataSize & 0xFF);
        header[41] = (byte)((dataSize >> 8) & 0xFF);
        header[42] = (byte)((dataSize >> 16) & 0xFF);
        header[43] = (byte)((dataSize >> 24) & 0xFF);

        // 合併檔案頭和音頻數據
        byte[] bytes = new byte[headerSize + dataSize];
        Buffer.BlockCopy(header, 0, bytes, 0, headerSize);

        // 將 Int16 數據轉換為字節數組
        byte[] soundData = new byte[dataSize];
        Buffer.BlockCopy(intData, 0, soundData, 0, dataSize);
        Buffer.BlockCopy(soundData, 0, bytes, headerSize, dataSize);

        Debug.Log($"生成的 WAV 檔案大小: {bytes.Length} bytes");
        Debug.Log($"音頻參數 - 採樣率: {sampleRate}Hz, 聲道數: {clip.channels}, 位元深度: 16-bit");

        return bytes;
    }
}

[Serializable]
public class TranscriptionResponse
{
    public string action;
    public string text;
}