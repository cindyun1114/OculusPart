using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public string sceneName; // 在Inspector中指定要切換的場景名稱
    public Button switchButton; // 指定按鈕

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
