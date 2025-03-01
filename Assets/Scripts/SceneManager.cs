using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public string sceneName; // �bInspector�����w�n�����������W��
    public Button switchButton; // ���w���s

    void Start()
    {
        if (switchButton != null)
        {
            switchButton.onClick.AddListener(SwitchScene);
        }
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
