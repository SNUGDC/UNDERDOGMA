using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private GameData _gameData;
    public GameData GameData => _gameData;

    private AchievementData _achievementData;
    public AchievementData AchievementData => _achievementData;

    protected override void Awake()
    {
        base.Awake();
    }

    // 게임 시작과 동시에 불러와야 하는 데이터들은 여기에서 부른다. 
    void Start()
    {

    }

    // 세이브파일 로드 시 불러와야 하는 데이터 등은 여기에서 부른다.
    public void LoadAllData(int saveFileNum)
    {
        _gameData = LoadGameData(saveFileNum);
        _achievementData = LoadAchievementData(saveFileNum);
    }

    public void SaveGameData(int saveFileNum)
    {
        GameDataLoader.Instance.saveData(_gameData, string.Format("GameData{0}", saveFileNum));
    }

    public GameData LoadGameData(int saveFileNum)
    {
        return GameDataLoader.Instance._load(string.Format("GameData{0}", saveFileNum));
    }

    public void SaveAchievementData(int saveFileNum)
    {
        AchievementDataLoader.Instance.saveData(_achievementData, string.Format("AchievementData{0}", saveFileNum));
    }

    public AchievementData LoadAchievementData(int saveFileNum)
    {
        return AchievementDataLoader.Instance._load(string.Format("AchievementData{0}", saveFileNum));
    }
}
