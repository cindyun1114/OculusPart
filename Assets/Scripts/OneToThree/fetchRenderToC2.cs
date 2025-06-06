using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class fetchRenderToC2 : MonoBehaviour
{
  private int CurrentCourseId;
  public string apiUrl = "https://feynman-server.onrender.com/get_chapters";

  public Transform chapterListContainer;
  public GameObject chapterPrefab;

  public Action<Chapter[]> OnChaptersFetched;
  public Slider progressBar;
  public TMP_Text progressText;

  public bool isDataReady = false;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    if (PersistentDataManager.Instance != null)
    {
      CurrentCourseId = PersistentDataManager.Instance.CurrentCourseId;
    }
    StartCoroutine(FetchChapters(CurrentCourseId, "classroom"));
  }

  // Update is called once per frame
  void Update()
  {

  }

  IEnumerator FetchChapters(int course_id, string chapter_type)
  {
    string apiUrl = $"{this.apiUrl}?course_id={course_id}&chapter_type={chapter_type}";

    using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
    {
      yield return request.SendWebRequest();

      if (request.result == UnityWebRequest.Result.Success)
      {
        Debug.Log("API Response: " + request.downloadHandler.text);
        string jsonResponse = "{\"chapters\":" + request.downloadHandler.text + "}";
        ChapterList chapterList = JsonUtility.FromJson<ChapterList>(jsonResponse);

        if (chapterList == null || chapterList.chapters == null)
        {
          Debug.LogError("Error: Parsed JSON is null. Check API response format.");
        }

        DisplayChapters(chapterList.chapters);
        // ✅ 通知有其他人想要知道資料完成的話
        OnChaptersFetched?.Invoke(chapterList.chapters);
      }
      else
      {
        Debug.LogError("Error fetching chapters: " + request.error);
      }
    }
  }

  void DisplayChapters(Chapter[] chapters)
  {
    int currentProgressBar = 0;
    if (chapterPrefab == null)
    {
      Debug.LogError("Error: chapterPrefab is null!");
      return;
    }

    if (chapterListContainer == null)
    {
      Debug.LogError("Error: chapterListContainer is null!");
      return;
    }

    foreach (Chapter chapter in chapters)
    {
      if (chapter.is_completed == 1)
      {
        currentProgressBar++;
      }
      GameObject newChapter = Instantiate(chapterPrefab, chapterListContainer);
      ChapterTitlePrefabScript component = newChapter.GetComponent<ChapterTitlePrefabScript>();
      component.SetData(chapter.chapter_name, chapter.is_completed);
      TMP_Text textComponent = newChapter.GetComponentInChildren<TMP_Text>();
    }

    progressBar.value = (float)currentProgressBar / chapters.Length;
    progressText.text = (100f * currentProgressBar / chapters.Length).ToString("F1") + "%";

    isDataReady = true;
    
  }
}

