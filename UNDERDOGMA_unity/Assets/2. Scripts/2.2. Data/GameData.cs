using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class GameData
{
    // 현재 무슨 챕터인지를 저장하는 변수. 월드맵과 같은 말이다. 
    [JsonIgnore] private int _currentWorld;
    public int CurrentWorld
    {
        get => _currentWorld;
        set => _currentWorld = value;
    }
    [JsonIgnore] private int _currentStage;
    public int CurrentStage
    {
        get => _currentStage;
        set => _currentStage = value;
    }

    public GameData(int currentWorld, int currentStage)
    {
        this._currentWorld = currentWorld;
        this._currentStage = currentStage;
    }
}

public class GameDataLoader : Singleton<GameDataLoader>
{
    // 게임 데이터를 저장한다.
    public bool saveData(GameData holder, string dataName)
    {
        string path = Application.streamingAssetsPath;
        path += $"/Data/Game/{dataName}.json";

        var converter = new StringEnumConverter();
        var pDataStringSave = JsonConvert.SerializeObject(holder, converter);
        File.WriteAllText(path, pDataStringSave);
        return true;
    }

    // 게임 데이터를 불러온다. 
    public GameData _load(string dataName)
    {
        string path = Application.streamingAssetsPath;
        path += $"/Data/Game/{dataName}.json";

        var converter = new StringEnumConverter();
        var pDataStringLoad = File.ReadAllText(path);
        GameData playerData = JsonConvert.DeserializeObject<GameData>(pDataStringLoad, converter);

        return playerData;
    }
}