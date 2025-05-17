using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class buttonManager : MonoBehaviour
{
    public GameObject settlementPanel;
    public GameObject settlementPanel1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toSettlementPanel1()
    {
        settlementPanel.gameObject.SetActive(false);
        settlementPanel1.gameObject.SetActive(true);
    }

    public void deactivateVR()
    {
        Debug.Log("Deactivate VR for course: " + PersistentDataManager.Instance.UserId);
        StartCoroutine(MarkCoureseVRZero(PersistentDataManager.Instance.CurrentCourseId, PersistentDataManager.Instance.UserId));
        SceneManager.LoadScene("LoginScene");
    }

    IEnumerator MarkCoureseVRZero(int courseId, int userId)
    {
        string url = "https://feynman-server.onrender.com/deactivate_VR";
        Debug.Log("The course ID: " + courseId);
        Debug.Log($"The user's Id: {userId}");

        string jsonData = JsonUtility.ToJson(new deactivateVR
        {
            action = "deactivate_VR",
            course_id = courseId,
            user_id = userId
        });

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Course marked as VR deactivated!");
            }
            else
                Debug.LogError("Failed to mark course: " + request.error);
        }

    }
}

[System.Serializable]
public class deactivateVR
{
    public string action;
    public int course_id;
    public int user_id;
}
