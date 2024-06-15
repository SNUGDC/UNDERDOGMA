using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveFileButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI ChapterText;
    [SerializeField] TextMeshProUGUI PlayTime;
    [SerializeField] GameObject ConnectBar;
    [SerializeField] Button DeleteButton;

    int saveFileNum;

    public void Init(int saveFileNum)
    {
        this.saveFileNum = saveFileNum;
    }

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
            DeleteButton.GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        }
        else
        {
            DeleteButton.GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (LoadGameSceneManager.Instance._selectedSaveFileNum != saveFileNum)
        {
            LoadGameSceneManager.Instance.CursorMove(LoadGameSceneManager.Instance._selectedSaveFileNum, saveFileNum, ref LoadGameSceneManager.Instance.isCursorOnDeleteButton);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {

    }
}