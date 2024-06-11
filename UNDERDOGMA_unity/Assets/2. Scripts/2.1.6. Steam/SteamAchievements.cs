using Steamworks;
using UnityEngine;

public class SteamAchievements : MonoBehaviour
{
    private static SteamAchievements instance;
    public static SteamAchievements Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SteamAchievements>();
                if (instance == null)
                {
                    GameObject go = new GameObject("SteamAchievements");
                    instance = go.AddComponent<SteamAchievements>();
                }
            }
            return instance;
        }
    }

    private Achievement[] achievements;
    private int numAchievements;
    private bool initialized;

    protected Callback<UserStatsReceived_t> userStatsReceived;
    protected Callback<UserStatsStored_t> userStatsStored;
    protected Callback<UserAchievementStored_t> achievementStored;

    private void OnEnable()
    {
        if (SteamManager.Initialized)
        {
            userStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
            userStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
            achievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

            if (!initialized)
            {
                RequestStats();
            }
        }
    }

    public void SetAchievements(Achievement[] achievements)
    {
        this.achievements = achievements;
        this.numAchievements = achievements.Length;
    }

    public bool RequestStats()
    {
        if (!SteamManager.Initialized)
        {
            return false;
        }

        if (!SteamUser.BLoggedOn())
        {
            return false;
        }

        return SteamUserStats.RequestCurrentStats();
    }

    public bool SetAchievement(string ID)
    {
        if (initialized)
        {
            SteamUserStats.SetAchievement(ID);
            return SteamUserStats.StoreStats();
        }
        return false;
    }

    private void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
        if ((ulong)SteamUtils.GetAppID() == pCallback.m_nGameID)
        {
            if (pCallback.m_eResult == EResult.k_EResultOK)
            {
                Debug.Log("Received stats and achievements from Steam");
                initialized = true;

                for (int i = 0; i < numAchievements; ++i)
                {
                    Achievement ach = achievements[i];

                    SteamUserStats.GetAchievement(ach.AchievementID.ToString(), out ach.Achieved);
                    ach.AchievementName = SteamUserStats.GetAchievementDisplayAttribute(ach.AchievementID.ToString(), "name");
                    ach.Description = SteamUserStats.GetAchievementDisplayAttribute(ach.AchievementID.ToString(), "desc");

                    achievements[i] = ach;
                }
            }
            else
            {
                Debug.LogError($"RequestStats - failed, {pCallback.m_eResult}");
            }
        }
    }

    private void OnUserStatsStored(UserStatsStored_t pCallback)
    {
        if ((ulong)SteamUtils.GetAppID() == pCallback.m_nGameID)
        {
            if (pCallback.m_eResult == EResult.k_EResultOK)
            {
                Debug.Log("Stored stats for Steam");
            }
            else
            {
                Debug.LogError($"StatsStored - failed, {pCallback.m_eResult}");
            }
        }
    }

    private void OnAchievementStored(UserAchievementStored_t pCallback)
    {
        if ((ulong)SteamUtils.GetAppID() == pCallback.m_nGameID)
        {
            Debug.Log("Stored Achievement for Steam");
        }
    }
}
