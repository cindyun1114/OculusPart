using UnityEngine;

public class PersistentDataManager : MonoBehaviour
{
    public static PersistentDataManager Instance;

    // �Τ�򥻸��
    public int UserId;
    public string Username;

    // �ҵ{���
    public int CurrentCourseId;
    public string CurrentCourseName;
    public string CurrentStage;

    // �i�׸��
    public float OneToOneProgress;
    public float ClassroomProgress;

    // ���`�έp
    public int CompletedChapters;
    public int TotalChapters;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // ������O�s
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �s��n�J��T�]�qVRLogin�ӡ^
    /// </summary>
    public void SetUserData(int userId, string username)
    {
        UserId = userId;
        Username = username;
    }

    /// <summary>
    /// �s��ҵ{��ơ]�qcurrent_stage�ӡ^
    /// </summary>
    public void SetCourseData(int courseId, string courseName, string stage, float oneToOneProgress, float classroomProgress)
    {
        CurrentCourseId = courseId;
        CurrentCourseName = courseName;
        CurrentStage = stage;
        OneToOneProgress = oneToOneProgress;
        ClassroomProgress = classroomProgress;
    }
}