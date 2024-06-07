using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

// 게임 시작을 누른 후, 세이브 파일을 선택하는 메뉴에 관한 스크립트. 
public class SelectGameMenu : MonoBehaviour
{
    private string _path;

    [SerializeField] private List<Button> GameButtons;

    public Button[] MenuButtons;
    public TextMeshProUGUI[] ButtonTexts;
    //List<string> list = new List<string>();

    [SerializeField] private Button ReturnToTitleButton;
    [SerializeField] private TextMeshProUGUI ReturnToTitleButtonText;
    [SerializeField] private Vector3 SelectedSaveFilePosition;
    [SerializeField] private GameObject TutorialPanel;
    [SerializeField] private Button TutorialPlayButton;
    [SerializeField] private Button SkipButton;
    [SerializeField] private Image LogoImage;
    [SerializeField] private Sprite[] WorldThemeSprites;

    private int selectedSaveFileNum;
    private List<SaveFileButton> SaveFileButtons = new List<SaveFileButton>();

    void Start()
    {
        SoundManager.Instance.PlayBgm(true);

        // 1. 파일의 경로를 지정해준다. 세부적인 path는 NewGame, LoadGame에서 조절해준다.
        // 2. SelectSaveFileButtons 리스트에 각 버튼의 스크립트를 할당해준다.
        // 3. 각각의 데이터들이 존재하는지 확인하고, 이에 따라 버튼 초기 설정을 해준다.
        for (int i = 0; i < 3; i++)
        {
            _path = Application.streamingAssetsPath;
            _path += string.Format("/Data/Game/GameData{0}.json", i);

            FileInfo fileInfo = new FileInfo(_path);
            SaveFileButtons.Add(GameButtons[i].gameObject.transform.GetComponent<SaveFileButton>());

            // 만약 데이터가 없으면 공백으로 둔다.
            if (!fileInfo.Exists)
            {
                SaveFileButtons[i].CurrentWorldText.SetText("EMPTY");
                SaveFileButtons[i].WorldThemeImage.sprite = WorldThemeSprites[0]; // Transparent
            }
            // 만약 데이터가 있으면 현재 월드, 현재 스테이지가 무엇인지 보여준다.
            else
            {
                GameData gameData = GameDataLoader.Instance._load(string.Format("GameData{0}", i));
                SaveFileButtons[i].CurrentWorldText.SetText("Current World: " + gameData.CurrentWorld);
                SaveFileButtons[i].WorldThemeImage.sprite = WorldThemeSprites[1]; // 데모 월드 이미지
            }

            int index = i;
            GameButtons[i].onClick.AddListener(() => GameButtonClick(index));
            SaveFileButtons[i].SaveFileButtonInit();
            SaveFileButtons[i].SaveFileButtonFadeIn();
        }

        // 게임 시작 화면으로 되돌아가는 버튼을 누르면 GameStartMenu로 돌아간다.
        ReturnToTitleButton.onClick.AddListener(returnToTitle);
        returnToTitleInit();

        selectedSaveFileNum = -1;
    }

    // 게임 버튼 하나를 클릭하면
    public void GameButtonClick(int saveFileNum)
    {
        // 나머지 게임 데이터들은 비활성화하고
        for (int i = 0; i < 3; i++)
        {
            if (i == saveFileNum)
            {
                //선택된 버튼은 중앙으로 이동하고, 크기가 커진다.
                selectedSaveFileNum = i;
                GameButtons[i].interactable = false;
                SaveFileButtons[i].IsInteractable = false;
                SaveFileButtons[i].SelectButtonCapsule(true);
                GameButtons[i].transform.DOLocalMove(SelectedSaveFilePosition, 0.5f).SetEase(Ease.InOutBack);
                SaveFileButtons[i].transform.DOScale(SaveFileButtons[i].InitialScale * 1.15f, 0.5f).SetEase(Ease.InOutBack);
                continue;
            }
            else
            {
                SaveFileButtons[i].SaveFileButtonFadeOut();
            }
        }

        //게임 시작 버튼 텍스트를 수정한다(Continue or New Game)
        _path = Application.streamingAssetsPath + string.Format("/Data/Game/GameData{0}.json", saveFileNum);
        FileInfo fileInfo = new FileInfo(_path);
        if (fileInfo.Exists)
        {
            Debug.Log("fileinfo exists");
            MenuButtons[0].GetComponentInChildren<TextMeshProUGUI>().SetText("CONTINUE");
        }
        else
        {
            Debug.Log("fileinfo doesn't exists");
            MenuButtons[0].GetComponentInChildren<TextMeshProUGUI>().SetText("NEW GAME");
        }
        // CONTINUE, RETURN, DELETE 버튼을 활성화하고 타이틀로 돌아가기 버튼을 비활성화한다.

        ToggleButtons(true);

        // 시작 버튼과 삭제 버튼에 각각 함수를 연결해준다.
        MenuButtons[0].onClick.AddListener(() => StartGame(saveFileNum));
        MenuButtons[1].onClick.AddListener(ReturnButtonClicked);
        MenuButtons[2].onClick.AddListener(() => DeleteGame(saveFileNum));

        //로고를 지운다.
        LogoImage.DOFade(0f, 0.5f);
    }

    public void ReturnButtonClicked()
    {
        int selectedSaveFileNumTemp = selectedSaveFileNum;

        // 뒤로 버튼을 클릭하면 게임 데이터들을 다시 보여주고
        for (int i = 0; i < 3; i++)
        {
            if (i == selectedSaveFileNum)
            {
                //버튼은 원위치로 이동하고, 크기도 원상태로 되돌아간다.
                GameButtons[i].interactable = false;
                SaveFileButtons[selectedSaveFileNum].SelectButtonCapsule(false);
                DG.Tweening.Sequence ButtonMove = DOTween.Sequence()
                    .Append(GameButtons[i].transform.DOLocalMove(SaveFileButtons[i].InitialPosition, 0.5f).SetEase(Ease.InOutBack))
                    .Join(SaveFileButtons[i].transform.DOScale(SaveFileButtons[i].InitialScale, 0.5f).SetEase(Ease.InOutBack))
                    .OnComplete(() =>
                    {
                        GameButtons[selectedSaveFileNumTemp].interactable = true;
                        SaveFileButtons[selectedSaveFileNumTemp].IsInteractable = true;
                    });
                selectedSaveFileNum = -1;
                continue;
            }
            else
            {
                SaveFileButtons[i].SaveFileButtonFadeIn();
            }
        }

        for (int i = 0; i < 3; i++)
        {
            MenuButtons[i].onClick.RemoveAllListeners();
        }

        // 시작 버튼과 뒤로 버튼, 삭제 버튼을 비활성화하고 타이틀로 돌아가기 버튼을 활성화한다.
        ToggleButtons(false);

        //로고를 표시한다.
        LogoImage.DOFade(1f, 0.5f);
    }

    // 기존에 존재하는 게임 데이터를 삭제하는 경우.
    public void DeleteGame(int saveFileNum)
    {
        // 세이브파일의 모든 데이터에 대해 경로를 지정하고
        List<string> paths = new List<string>
        {
            Application.streamingAssetsPath + string.Format("/Data/Game/GameData{0}.json", saveFileNum),
            Application.streamingAssetsPath + string.Format("/Data/Achievement/AchievementData{0}.json", saveFileNum),
        };

        // 각각을 모두 지워준다.
        foreach (string path in paths)
        {
            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.Exists)
            {
                File.Delete(path);
            }
        }

        // 보여지는 것을 변경한다.
        SaveFileButtons[saveFileNum].CurrentWorldText.SetText("EMPTY");
        MenuButtons[0].GetComponentInChildren<TextMeshProUGUI>().SetText("NEW GAME");
    }

    // 게임 시작 버튼을 눌렀을 때, 로드가 아니라 처음 시작하는거라면 튜토리얼을 스킵할지 물어보는 창을 띄워준다. 
    public async void StartGame(int saveFileNum)
    {
        _path = Application.streamingAssetsPath + string.Format("/Data/Game/GameData{0}.json", saveFileNum);

        FileInfo fileInfo = new FileInfo(_path);
        Debug.Log(fileInfo);

        // 게임 데이터가 없다면 새로운 데이터를 만들어주고
        if (!fileInfo.Exists)
        {
            await CreateSaveFile(saveFileNum, isTutorial: true);
        }

        GameManager.Instance.SaveFileNum = saveFileNum;
        DataManager.Instance.LoadAllData(saveFileNum);
        LoadingManager.Instance.LoadScene("Stage", false, false, 0, 1);
    }


    // 새로운 게임이 시작되는 경우, 기본적인 데이터들을 설정해준다. 
    public async Task CreateSaveFile(int saveFileNum, bool isTutorial)
    {
        // 1. 게임 데이터를 복사해준다.
        await Extensions.CopyAsync(Application.streamingAssetsPath + "/Data/Game/DefaultGameData.json",
            Application.streamingAssetsPath + string.Format("/Data/Game/GameData{0}.json", saveFileNum));

        // 2. 기본 플레이어 데이터를 복사해주고
        await Extensions.CopyAsync(Application.streamingAssetsPath + "/Data/Achievement/DefaultAchievementData.json",
        Application.streamingAssetsPath + string.Format("/Data/Achievement/AchievementData{0}.json", saveFileNum));
    }

    public void ToggleButtons(bool isOn)
    {
        if (isOn)
        {
            DG.Tweening.Sequence ToggleTrue = DOTween.Sequence()
                .Append(ButtonTexts[0].DOFade(1f, 0.5f))
                .Join(ButtonTexts[1].DOFade(1f, 0.5f))
                .Join(ButtonTexts[2].DOFade(1f, 0.5f))
                .Join(ReturnToTitleButtonText.DOFade(0f, 0.5f))
                .OnComplete(() =>
                {
                    MenuButtons[0].interactable = true;
                    MenuButtons[1].interactable = true;
                    MenuButtons[2].interactable = true;
                    ReturnToTitleButton.gameObject.SetActive(false);
                });
        }
        else
        {
            DG.Tweening.Sequence ToggleFalse = DOTween.Sequence()
                .OnStart(() =>
                {
                    MenuButtons[0].interactable = false;
                    MenuButtons[1].interactable = false;
                    MenuButtons[2].interactable = false;
                    ReturnToTitleButton.gameObject.SetActive(true);
                })
                .Append(ButtonTexts[0].DOFade(0f, 0.5f))
                .Join(ButtonTexts[1].DOFade(0f, 0.5f))
                .Join(ButtonTexts[2].DOFade(0f, 0.5f))
                .Join(ReturnToTitleButtonText.DOFade(1f, 0.5f));
        }
    }

    public void returnToTitleInit()
    {
        ReturnToTitleButton.interactable = false;
        ReturnToTitleButtonText.color = new Color(ReturnToTitleButtonText.color.r, ReturnToTitleButtonText.color.g, ReturnToTitleButtonText.color.b, 0f);
        ReturnToTitleButtonText.DOFade(1f, 0.5f).OnComplete(() => { ReturnToTitleButton.interactable = true; });
    }

    // 게임 시작 화면으로 되돌아가는 버튼. 
    public void returnToTitle()
    {
        SceneManager.LoadScene("GameStartMenu");
    }
}