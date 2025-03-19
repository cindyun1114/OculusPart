using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.InputSystem;

public class chatWithProfessor : MonoBehaviour
{
    public TMP_Text blackBoardText;
    public TMP_InputField testInput;

    private string apiFetchUrl = "https://feynman-server.onrender.com/fetch";
    private string apiChatUrl = "https://feynman-server.onrender.com/chat";
    private int UserId;
    private string Username;
    private int CurrentCourseId;
    private string CurrentCourseName;
    private string CurrentStage;
    private float OneToOneProgress;
    private float ClassroomProgress;
    private int CompletedChapters;
    private int TotalChapters;

    public string assistant_id = "";
    public string thread_id = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PersistentDataManager.Instance != null)
        {
            UserId = PersistentDataManager.Instance.UserId;
            Username = PersistentDataManager.Instance.Username;
            CurrentCourseId = PersistentDataManager.Instance.CurrentCourseId;
            CurrentCourseName = PersistentDataManager.Instance.CurrentCourseName;
            CurrentStage = PersistentDataManager.Instance.CurrentStage;
            OneToOneProgress = PersistentDataManager.Instance.OneToOneProgress;
            ClassroomProgress = PersistentDataManager.Instance.ClassroomProgress;
            CompletedChapters = PersistentDataManager.Instance.CompletedChapters;
            TotalChapters = PersistentDataManager.Instance.TotalChapters;

            StartCoroutine(getChatGPTIDs());
        }
        else
        {
            Debug.LogError("PersistentDataManager.Instance is null! 確保 PersistentDataManager 存在於場景中。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame) // 檢測 Enter 鍵
        {
            Debug.Log("Sending a Message.");
            StartCoroutine(SendMessage());
        }
    }

    IEnumerator getChatGPTIDs()
    {
        Debug.Log("開始從資料庫載入資料...");

        string jsonData = JsonUtility.ToJson(new fetchRequest
        {
            action = "fetch_assistant_and_thread",
            course_id = CurrentCourseId,
            role = "teacher"
        });

        using (UnityWebRequest request = new UnityWebRequest(apiFetchUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");


            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<fetchResponse>(request.downloadHandler.text);
                assistant_id = response.assistant_id;
                thread_id = response.thread_id;
                Debug.Log("assistant_Id and thread_id fetched successfully");
                Debug.Log("assistant_Id: " + assistant_id);
                Debug.Log("thread_id: " + thread_id);
            }
            else
            {
                blackBoardText.text = "Error: " + request.error;
            }
        }
    }

    public IEnumerator SendMessage()
    {

        string jsonData = JsonUtility.ToJson(new messageRequest
        {
            action = "message",
            message = testInput.text,
            assistant_id = assistant_id,
            thread_id = thread_id
        });
        testInput.text = "";

        using (UnityWebRequest request = new UnityWebRequest(apiChatUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");


            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var response = JsonUtility.FromJson<messageResponse>(request.downloadHandler.text);
                blackBoardText.text = response.message;
                Debug.Log(response.message);
            }
            else
            {

                blackBoardText.text = "Error: " + request.error;
            }
        }
    }
}

[System.Serializable]//�o�����O �i�ǦC�ƪ�
public class fetchRequest
{
    public string action;
    public int course_id;
    public string role;
}

[System.Serializable]
public class fetchResponse
{
    public string action;
    public string assistant_id;
    public string thread_id;
}

[System.Serializable]
public class messageResponse
{
    public string action;
    public string message;
}

[System.Serializable]
public class messageRequest
{
    public string action;
    public string assistant_id;
    public string thread_id;
    public string message;
}


