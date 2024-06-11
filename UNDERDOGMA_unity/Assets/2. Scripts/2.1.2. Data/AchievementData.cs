using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class AchievementData
{
    // 현재 무슨 챕터인지를 저장하는 변수. 월드맵과 같은 말이다. 
    [JsonIgnore] private int _killCount;
    public int KillCount
    {
        get => _killCount;
        set => _killCount = value;
    }

    public AchievementData(int killCount)
    {
        this._killCount = killCount;
    }
}

public class AchievementDataLoader : Singleton<AchievementDataLoader>
{
    // 게임 데이터를 저장한다.
    public bool saveData(AchievementData holder, string dataName)
    {
        string path = Application.streamingAssetsPath;
        path += $"/Data/Achievement/{dataName}.json";

        var converter = new StringEnumConverter();
        var pDataStringSave = JsonConvert.SerializeObject(holder, converter);
        File.WriteAllText(path, pDataStringSave);
        return true;
    }

    // 게임 데이터를 불러온다. 
    public AchievementData _load(string dataName)
    {
        string path = Application.streamingAssetsPath;
        path += $"/Data/Achievement/{dataName}.json";

        var converter = new StringEnumConverter();
        var pDataStringLoad = File.ReadAllText(path);
        AchievementData achievementData = JsonConvert.DeserializeObject<AchievementData>(pDataStringLoad, converter);

        return achievementData;
    }
}