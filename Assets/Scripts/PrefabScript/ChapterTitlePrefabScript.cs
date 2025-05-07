using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChapterTitlePrefabScript : MonoBehaviour
{
    public TMP_Text titleText;
    public Image checkImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetData(string name, int isCompleted)
    {
        titleText.text = name;

        if (isCompleted == 1)
        {
            checkImage.gameObject.SetActive(true);
        }

    }
}
