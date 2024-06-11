using TMPro;
using UnityEngine;

public class SaveFileButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChapterText;
    [SerializeField] TextMeshProUGUI PlayTime;

    public void SetChapterText(string text)
    {
        ChapterText.SetText(text);
    }

    public void SetPlayTime(string text)
    {
        PlayTime.SetText(text);
    }
}