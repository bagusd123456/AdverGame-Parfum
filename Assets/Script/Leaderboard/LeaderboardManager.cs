using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class LeaderboardManager : MonoBehaviour
{
    public string playerName = "undefined";

    [Button]
    public void SetPlayerName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = playerName
        }, result =>
        {
            Debug.Log($"Player name set to: {playerName}");
        }, error =>
        {
            Debug.Log($"Error set name");
        });
    }

    // Start is called before the first frame update
    void Awake()
    {
        Login();
    }

    async void Login()
    {
        var request = new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,

        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    static void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create!");
    }

    static void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }

    public static void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScore",
                    Value = score
                }
            }
        };
        
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    static void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard sent!");
    }

    [Button]
    public static void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    [Button]
    public void SaveScore()
    {
        SendLeaderboard(10);
    }

    static void OnLeaderboardGet(GetLeaderboardResult result)
    {
        Debug.Log("Successful leaderboard get!");
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.DisplayName + " : " + item.StatValue);
            Debug.Log($"Player Name: {item.DisplayName}\nScore is: {item.StatValue}");
        }
    }

    static void SavePlayerData(UpdateUserDataRequest request)
    {
        PlayFabClientAPI.UpdateUserData(request, 
            (result) => { Debug.Log($"Data Send Successfully"); },
            (error) => { Debug.Log($"Data Send Error"); });
    }
}
