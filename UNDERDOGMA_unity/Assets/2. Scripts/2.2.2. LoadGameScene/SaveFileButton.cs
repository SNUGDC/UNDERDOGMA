using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChapterText;
    [SerializeField] TextMeshProUGUI PlayTime;
    [SerializeField] GameObject ConnectBar;
    [SerializeField] Button DeleteButton;

    public void SetChapterText(string text)
    {
        ChapterText.SetText(text);
    }

    public void SetPlayTime(string text)
    {
        PlayTime.SetText(text);
    }

    public void ToggleDeleteMenu(bool isOn)
    {
        ConnectBar.SetActive(isOn);
        DeleteButton.gameObject.SetActive(isOn);
    }

    public void HoverDeleteMenu(bool isOn)
    {
        if (isOn)
        {
            ConnectBar.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            DeleteButton.GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        else
        {
            ConnectBar.transform.localScale = new Vector3(1, 1, 1);
            DeleteButton.GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(1, 1, 1);
        }
    }
}