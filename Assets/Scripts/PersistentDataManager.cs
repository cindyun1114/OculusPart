using UnityEngine;

public class PersistentDataManager : MonoBehaviour
{
    public static PersistentDataManager Instance;

    // �Τ���
    public int UserId;
    public string Username;

    // �ҵ{���
    public int CurrentCourseId;
    public string CurrentCourseName;
    public string CurrentStage;
    public int TeacherCardId;  // �s�W�G�Ѯv�d��ID

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
            DontDestroyOnLoad(gameObject);  // ���P������
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �x�s�n�J��ơ]�qVRLogin�����^
    /// </summary>
    public void SetUserData(int userId, string username)
    {
        UserId = userId;
        Username = username;
    }

    /// <summary>
    /// �x�s�ҵ{��ơ]�qcurrent_stage�����^
    /// </summary>
    public void SetCourseData(int courseId, string courseName, string stage, float oneToOneProgress, float classroomProgress, int teacherCardId)
    {
        CurrentCourseId = courseId;
        CurrentCourseName = courseName;
        CurrentStage = stage;
        OneToOneProgress = oneToOneProgress;
        ClassroomProgress = classroomProgress;
        TeacherCardId = teacherCardId;  // �s�W�G�]�m�Ѯv�d��ID
        Debug.Log($"[PersistentDataManager] 設置 TeacherCardId: {TeacherCardId}");
    }
}