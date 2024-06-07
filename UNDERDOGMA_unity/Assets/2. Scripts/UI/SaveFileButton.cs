using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveFileButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image ButtonImage;
    public Image WorldThemeImage;
    public TextMeshProUGUI SaveFileName;
    public TextMeshProUGUI CurrentWorldText;
    [Header("World Sprites")]
    [SerializeField] private Sprite SelectedWorldSprite;
    [SerializeField] private Sprite UnselectedWorldSprite;

    [HideInInspector] public Vector3 InitialScale;
    [HideInInspector] public Vector3 InitialPosition;
    [HideInInspector] public bool IsInteractable;

    private void Start()
    {
        InitialScale = ButtonImage.transform.localScale;
        InitialPosition = transform.localPosition;
        IsInteractable = true;
    }

    public void SaveFileButtonFadeIn()
    {
        Sequence ButtonFadeIn = DOTween.Sequence()
            .OnStart(() =>
            {
                gameObject.SetActive(true);
                gameObject.GetComponent<Button>().interactable = false;
            })
            .Append(ButtonImage.DOFade(1f, 0.5f))
            .Join(WorldThemeImage.DOFade(1f, 0.5f))
            .Join(SaveFileName.DOFade(1f, 0.5f))
            .Join(CurrentWorldText.DOFade(1f, 0.5f))
            .OnComplete(() =>
            {
                gameObject.GetComponent<Button>().interactable = true;
            });
    }
    public void SaveFileButtonFadeOut()
    {
        Sequence ButtonFadeOut = DOTween.Sequence()
            .OnStart(() =>
            {
                gameObject.GetComponent<Button>().interactable = false;
            })
            .Append(ButtonImage.DOFade(0f, 0.5f))
            .Join(WorldThemeImage.DOFade(0f, 0.5f))
            .Join(SaveFileName.DOFade(0f, 0.5f))
            .Join(CurrentWorldText.DOFade(0f, 0.5f))
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }

    public void SaveFileButtonInit()
    {
        gameObject.GetComponent<Button>().interactable = false;
        ButtonImage.color = new Color(ButtonImage.color.r, ButtonImage.color.g, ButtonImage.color.b, 0f);
        WorldThemeImage.color = new Color(WorldThemeImage.color.r, WorldThemeImage.color.g, WorldThemeImage.color.b, 0f);
        SaveFileName.color = new Color(SaveFileName.color.r, SaveFileName.color.g, SaveFileName.color.b, 0f);
        CurrentWorldText.color = new Color(CurrentWorldText.color.r, CurrentWorldText.color.g, CurrentWorldText.color.b, 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsInteractable)
        {
            gameObject.transform.DOScale(InitialScale * 1.05f, 0.25f);
            SelectButtonCapsule(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsInteractable)
        {
            gameObject.transform.DOScale(InitialScale, 0.25f);
            SelectButtonCapsule(false);
        }
    }

    public void SelectButtonCapsule(bool isSelected)
    {
        ButtonImage.sprite = isSelected ? SelectedWorldSprite : UnselectedWorldSprite;
    }
}
