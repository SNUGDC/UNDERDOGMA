using System;
using UnityEngine;

public enum EAchievements
{
    NEW_ACHIEVEMENT_1_1 = 0,
    NEW_ACHIEVEMENT_1_2 = 1
}

[Serializable]
public struct Achievement
{
    public EAchievements AchievementID;
    public string AchievementName;
    public string Description;
    public bool Achieved;
    public int IconImage;
}

public class AchievementManager : Singleton<AchievementManager>
{
    public Achievement[] achievements;

    void Start()
    {
        achievements = new Achievement[]
        {
            new Achievement { AchievementID = EAchievements.NEW_ACHIEVEMENT_1_1, AchievementName = "5Kill", Description = "", Achieved = false, IconImage = 0 },
            new Achievement { AchievementID = EAchievements.NEW_ACHIEVEMENT_1_2, AchievementName = "Executed", Description = "", Achieved = false, IconImage = 0 },
        };

        // 전역 액세스 설정
        SteamAchievements.Instance.SetAchievements(achievements);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SteamAchievements.Instance.SetAchievement("NEW_ACHIEVEMENT_1_2");
        }
    }

    public void KillFiveEnemies(int killCount)
    {
        Debug.Log("killcount : " + killCount);
        SteamAchievements.Instance.ResetAllAchievements();
        if (killCount >= 5)
        {
            SteamAchievements.Instance.SetAchievement("NEW_ACHIEVEMENT_1_1");
        }
    }
}
