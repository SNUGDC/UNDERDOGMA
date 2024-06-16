using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeleteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => LoadGameSceneManager.Instance.DeleteMenuPopUp(true));
    }

    public void HoverDeleteMenu(bool isOn)
    {
        if (isOn)
        {
            GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        }
        else
        {
            GetComponentInChildren<TextMeshProUGUI>().transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        HoverDeleteMenu(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        HoverDeleteMenu(false);
    }
}