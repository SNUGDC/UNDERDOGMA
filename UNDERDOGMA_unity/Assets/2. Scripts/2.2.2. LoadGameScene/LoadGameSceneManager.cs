using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;

// 게임 시작을 누른 후, 세이브 파일을 선택하는 메뉴에 관한 스크립트. 
public class LoadGameSceneManager : Singleton<LoadGameSceneManager>
{
    private string _path;

    [SerializeField] private List<SaveFileButton> SaveFileButtons;

    [SerializeField] private Button ReturnToTitleButton;
    [SerializeField] private GameObject Cursor;
    [SerializeField] private GameObject DeletePanel;

    public int _selectedSaveFileNum;
    private List<Vector3> cursorPositions = new List<Vector3>
    {
        new Vector3(-502, 220, 0),
        new Vector3(-502, 0, 0),
        new Vector3(-502, -220, 0)
    };

    public bool isCursorOnDeleteButton = false;

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

            // 만약 데이터가 없으면 공백으로 둔다.
            if (!fileInfo.Exists)
            {
                SaveFileButtons[i].SetChapterText("");
                SaveFileButtons[i].SetPlayTime("");
            }
            // 만약 데이터가 있으면 현재 월드, 현재 스테이지가 무엇인지 보여준다.
            else
            {
                GameData gameData = GameDataLoader.Instance._load(string.Format("GameData{0}", i));
                SaveFileButtons[i].SetChapterText("Chapter " + gameData.HighestWorldCleared);
                SaveFileButtons[i].SetPlayTime("Play Time: " + gameData.PlayTime);
            }

            int index = i;

            SaveFileButtons[i].GetComponent<Button>().onClick.AddListener(() => StartGame(index));
            SaveFileButtons[i].Init(index);
        }

        // 게임 시작 화면으로 되돌아가는 버튼을 누르면 GameStartMenu로 돌아간다.
        ReturnToTitleButton.onClick.AddListener(returnToTitle);

        _selectedSaveFileNum = 0;
        Cursor.transform.localPosition = cursorPositions[0];

        SaveFileButtons[0].ToggleDeleteMenu(true);
        SaveFileButtons[1].ToggleDeleteMenu(false);
        SaveFileButtons[2].ToggleDeleteMenu(false);

        DeletePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (_selectedSaveFileNum < 2)
            {
                CursorMove(_selectedSaveFileNum, _selectedSaveFileNum + 1, ref isCursorOnDeleteButton);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (_selectedSaveFileNum > 0)
            {
                CursorMove(_selectedSaveFileNum, _selectedSaveFileNum - 1, ref isCursorOnDeleteButton);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            isCursorOnDeleteButton = true;
            SaveFileButtons[_selectedSaveFileNum].HoverDeleteMenu(true);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            isCursorOnDeleteButton = false;
            SaveFileButtons[_selectedSaveFileNum].HoverDeleteMenu(false);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isCursorOnDeleteButton == true)
            {
                DeletePanel.SetActive(true);
                GameObject clonedSaveFileButtonObject = Instantiate(SaveFileButtons[_selectedSaveFileNum].gameObject, SaveFileButtons[_selectedSaveFileNum].transform.position, Quaternion.identity);
                clonedSaveFileButtonObject.transform.SetParent(DeletePanel.transform);
                clonedSaveFileButtonObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f).SetEase(Ease.OutQuint);
                clonedSaveFileButtonObject.transform.DOLocalMove(new Vector3(-1f, -10f, 0), 0.2f).SetEase(Ease.OutQuint);
                clonedSaveFileButtonObject.GetComponent<SaveFileButton>().ToggleDeleteMenu(false);
            }
            else
            {
                StartGame(_selectedSaveFileNum);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            returnToTitle();
        }
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
    }

    public async void StartGame(int saveFileNum)
    {
        _path = Application.streamingAssetsPath + string.Format("/Data/Game/GameData{0}.json", saveFileNum);

        FileInfo fileInfo = new FileInfo(_path);
        Debug.Log(fileInfo);

        // 게임 데이터가 없다면 새로운 데이터를 만들어주고
        if (!fileInfo.Exists)
        {
            await CreateSaveFile(saveFileNum);
        }

        GameManager.Instance.SaveFileNum = saveFileNum;
        DataManager.Instance.LoadAllData(saveFileNum);
        LoadingManager.Instance.LoadScene("Stage", false, false, 1, 1);
    }

    // 새로운 게임이 시작되는 경우, 기본적인 데이터들을 설정해준다. 
    public async Task CreateSaveFile(int saveFileNum)
    {
        // 1. 기본 게임 데이터를 복사해준다.
        await Extensions.CopyAsync(Application.streamingAssetsPath + "/Data/Game/DefaultGameData.json",
            Application.streamingAssetsPath + string.Format("/Data/Game/GameData{0}.json", saveFileNum));

        // 2. 기본 업적 데이터를 복사해주고
        await Extensions.CopyAsync(Application.streamingAssetsPath + "/Data/Achievement/DefaultAchievementData.json",
        Application.streamingAssetsPath + string.Format("/Data/Achievement/AchievementData{0}.json", saveFileNum));
    }

    public void CursorMove(int selectedSaveFileNum, int targetSaveFileNum, ref bool isCursorOnDeleteButton)
    {
        if (isCursorOnDeleteButton == true)
        {
            isCursorOnDeleteButton = false;
        }

        SaveFileButtons[selectedSaveFileNum].ToggleDeleteMenu(false);
        SaveFileButtons[targetSaveFileNum].ToggleDeleteMenu(true);

        Cursor.transform.DOLocalMove(cursorPositions[targetSaveFileNum], 0.2f);

        _selectedSaveFileNum = targetSaveFileNum;
    }

    // 게임 시작 화면으로 되돌아가는 버튼. 
    public void returnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}