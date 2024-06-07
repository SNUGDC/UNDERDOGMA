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

public class AchievementManager : MonoBehaviour
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
}
