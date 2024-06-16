using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : Singleton<LoadingManager>
{
    // 1. 프리팹들을 할당하는 변수들. 
    [SerializeField] GameObject StageManagerPrefab;
    [SerializeField] GameObject ExecutionPrefab;
    [SerializeField] GameObject DialogueManagerPrefab;

    // 2. 만들어진 매니저들을 저장하기 위한 변수들.
    private GameObject stageManager;
    private GameObject executionManager;
    private GameObject dialogueManager;

    protected override void Awake()
    {
        base.Awake();
    }

    // 다른 씬으로 이동하는 함수. 
    public void LoadScene(string sceneName, bool gameDataSaving, bool achievementDataSaving, int world = -1, int stage = -1)
    {
        // 만약 스테이지로 들어가는 경우 어느 world와 stage인지를 설정해준다.
        if (world != -1 && stage != -1)
        {
            GameManager.Instance.World = world;
            GameManager.Instance.Stage = stage;
        }

        // Stage 씬으로 이동하고,
        StartCoroutine(LoadSceneAsync(sceneName, world, stage, gameDataSaving, achievementDataSaving));
    }

    private IEnumerator LoadSceneAsync(string sceneName, int world, int stage,
                                        bool gameDataSaving, bool achievementDataSaving)
    {
        // 1. 비동기적으로 Scene을 로드합니다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 2. 만약 게임 데이터를 저장해야 한다면 저장합니다.
        if (gameDataSaving)
            DataManager.Instance.SaveGameData(GameManager.Instance.SaveFileNum);

        // 3. 만약 업적 데이터를 저장해야 한다면 저장합니다.
        if (achievementDataSaving)
            DataManager.Instance.SaveAchievementData(GameManager.Instance.SaveFileNum);

        // 4. Scene 로드가 완료될 때까지 대기합니다.
        while (!asyncLoad.isDone)
        {
            // 로드 진행률을 확인하거나 다른 작업을 수행할 수 있습니다.
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // progress 값은 0.0에서 0.9 사이입니다.

            // 다음 프레임까지 대기합니다.
            yield return null;
        }

        // 5. 만약 스테이지로 들어가는 경우, StageManager, ExecutionManager, DialogueManager를 생성해준다.
        if (world != -1 && stage != -1)
        {
            stageManager = Instantiate(StageManagerPrefab);
            executionManager = Instantiate(ExecutionPrefab);
            dialogueManager = Instantiate(DialogueManagerPrefab);

            InitManagers(world, stage);
        }

        // 6. 로딩이 끝나고 해상도를 설정해준다.
        GameManager.Instance.SetResolution();
    }

    // 스테이지에서 다른 스테이지로 이동하는 함수.
    // 이 함수는 StageManager에서 호출된다. 기존의 StageManager와 Execution, DialogueManager는 삭제된다. 그리고 다시 생성한다.
    public void LoadNextStage(int nextWorld, int nextStage)
    {
        GameManager.Instance.World = nextWorld;
        GameManager.Instance.Stage = nextStage;

        InitManagers(nextWorld, nextStage);
    }

    private void InitManagers(int world, int stage)
    {
        stageManager.GetComponent<StageManager>().Init(world, stage);
        executionManager.GetComponent<ExecutionManager>().Init(world, stage);
        dialogueManager.GetComponent<DialogueManager>().Init(DialogueEvent.Start, GameManager.Instance.Language, world, stage);
    }
}