using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    #region Singleton
    // 싱글톤 패턴.
    // 싱글톤 클래스를 구현해두긴 했지만, stageManager와 executionMangaer, DialogueManager는 
    // dontdestroyonload가 필요없기 때문에 클래스 내부에 싱글톤 패턴을 간단히 구현.
    private static TutorialManager _instance;

    public static TutorialManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (TutorialManager)FindObjectOfType(typeof(TutorialManager));
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject($"{typeof(TutorialManager)} (Singleton)");
                    _instance = singletonObject.AddComponent<TutorialManager>();
                    singletonObject.transform.parent = null;
                }
            }

            return _instance;
        }
    }

    #endregion

    // 현재 텍스트를 담아두는 변수. 
    [SerializeField] private Canvas DialogueCanvas;
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private UnityEngine.UI.Image DialogueWindow;
    [SerializeField] private UnityEngine.UI.Image DialogueImage;
    [SerializeField] private UnityEngine.UI.Image DialogueArrow;

    // 게임 처음에 나오는 불꽃 애니메이션.
    [SerializeField] private GameObject flame;

    // 대사와 함께 나올 스탠딩 cg들을 저장하는 배열. 
    [SerializeField] private List<Sprite> listSprites;

    private DialogueData _dialogueData;

    //  현재 몇번째 텍스트를 읽고 있는지 저장하기 위한 변수.
    private int count = 0;

    public void Start()
    {
        string path = "Tutorial";
        _dialogueData = DialogueDataLoader.Instance.LoadDialogueData(path);

        // 불꽃 애니메이션
        flame.GetComponent<SpriteRenderer>().DOFade(1.0f, 5.0f).OnComplete(() =>
        {
            StartCoroutine(StartDialogueCoroutine());
        });
    }

    private void Update()
    {
        // 대화를 읽는다.
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            count++;

            if (count == 9)
            {
                flame.SetActive(false);
                DialogueWindow.enabled = false;
                DialogueImage.enabled = true;
            }

            // 모든 대화를 읽은 경우
            if (count == _dialogueData.DialogueList.Count)
            {
                // 모든 코루틴 종료
                StopAllCoroutines();
                ExitDialogue();
            }
            else
            {
                // 아닌 경우 다음 다이얼로그 출력.
                StopAllCoroutines();
                StartCoroutine(StartDialogueCoroutine());
            }
        }
    }

    public IEnumerator StartDialogueCoroutine()
    {
        DialogueArrow.enabled = false;

        DialogueText.text = "";

        if (listSprites[count] == null)
        {
            DialogueImage.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            DialogueImage.sprite = null;
        }
        else
        {
            DialogueImage.GetComponent<RectTransform>().sizeDelta = listSprites[count].rect.size;
            DialogueImage.sprite = listSprites[count];
        }

        // 텍스트 출력 //
        for (int i = 0; i < _dialogueData.DialogueList[count].Length; i++)
        {
            DialogueText.text += _dialogueData.DialogueList[count][i]; // 한글자씩 출력
            yield return new WaitForSeconds(0.01f);
        }

        if (count < 9)
        {
            DialogueArrow.enabled = true;
        }
    }

    public void ExitDialogue()
    {
        //// 초기화 ////
        count = 0;
        DialogueText.text = "";
        // 리스트 초기화 //
        _dialogueData.DialogueList.Clear();
        listSprites.Clear();
        // 애니메이터 초기화 //

        DialogueText.GetComponent<TextMeshProUGUI>().enabled = false;
        DialogueWindow.GetComponent<UnityEngine.UI.Image>().enabled = false;

        SceneManager.LoadScene("Stage1");
    }
}