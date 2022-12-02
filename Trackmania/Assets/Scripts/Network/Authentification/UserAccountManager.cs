using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;
using System;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;
    public static UnityEvent OnSignInSuccess = new UnityEvent();
    public static UnityEvent<string> OnSignInFailed = new UnityEvent<string>();
    public static UnityEvent<string> OnRegisterFailed = new UnityEvent<string>();

    public static UnityEvent<string, string> OnUserDataRetrieved = new UnityEvent<string, string>();
    public static UnityEvent<string, int> OnStatisticRetrieved = new UnityEvent<string, int>();
    public static UnityEvent<string, List<PlayerLeaderboardEntry>> OnLeaderboardRetrieved = new UnityEvent<string, List<PlayerLeaderboardEntry>>();
    public string playfabID;
    public string playerName;

    public GameObject rowPrefab;
    public Transform rowsParent;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnSignInSuccess.AddListener(()=> StartCoroutine(GameViewStart()));
        OnSignInSuccess.AddListener(SubmitName);
    }

    public void CreateAccount(string userName, string emailAddresse, string password)
    {
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest()
            {
                Email = emailAddresse,
                Password = password,
                Username = userName,
                RequireBothUsernameAndEmail = true
            },
            response =>
            {
                Debug.Log($"Succesful Account Creation : {userName}, {emailAddresse}");
                SignIn(userName, password);
            },
            error =>
            {
                Debug.Log($"Unsuccesful Account Creation: {userName}, {emailAddresse}");
                OnRegisterFailed.Invoke(error.ErrorMessage);
            }

        );

    }

    public void SignIn(string usernName, string password)
    {
        PlayFabClientAPI.LoginWithPlayFab(
            new LoginWithPlayFabRequest()
            {
                Username = usernName,
                Password = password
            },
            response =>
            {
                Debug.Log($"Successful Account Login: {usernName}");
                playfabID = response.PlayFabId;
                playerName = usernName;
                OnSignInSuccess.Invoke();
            },
            error =>
            {
                Debug.Log($"Unsuccessful Account Login: {usernName}");
                OnSignInFailed.Invoke(error.ErrorMessage);
            }
            );
    }


    void GetDeviceID(out string android_id, out string ios_id, out string custom_id)
    {
        android_id = string.Empty;
        ios_id = string.Empty;
        custom_id = string.Empty;

        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
            android_id = secure.CallStatic<string>("getString", contentResolver, "android_id");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ios_id = UnityEngine.iOS.Device.vendorIdentifier;
        }
        else
        {
            custom_id = SystemInfo.deviceUniqueIdentifier;
        }
    }

    public void SignInWithDevice()
    {
        GetDeviceID(out string android_id, out string ios_id, out string custom_id);

        if (!string.IsNullOrEmpty(android_id))
        {
            Debug.Log($"Logging in with Android Device");
            PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
            {
                AndroidDeviceId = android_id,
                OS = SystemInfo.operatingSystem,
                AndroidDevice = SystemInfo.deviceModel,
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true
            }, response =>
            {
                Debug.Log($"Success Login with Android Device ID");
                playfabID = response.PlayFabId;
                playerName = playfabID;
                OnSignInSuccess.Invoke();

            }, error =>
            {
                Debug.Log($"Unsuccess Login with Android Device ID: {error.ErrorMessage}");
                OnSignInFailed.Invoke(error.ErrorMessage);
            });
        }
        else if (!string.IsNullOrEmpty(ios_id))
        {
            Debug.Log($"Logging in with IOS Device");
            PlayFabClientAPI.LoginWithIOSDeviceID(new LoginWithIOSDeviceIDRequest()
            {
                DeviceId = ios_id,
                OS = SystemInfo.operatingSystem,
                DeviceModel = SystemInfo.deviceModel,
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true
            }, response =>
            {
                Debug.Log($"Success Login with IOS Device ID");
                playerName = playfabID;
                playfabID = response.PlayFabId;
                OnSignInSuccess.Invoke();

            }, error =>
            {
                Debug.Log($"Unsuccess Login with IOS Device ID: {error.ErrorMessage}");
                OnSignInFailed.Invoke(error.ErrorMessage);
            });
        }
        else if (!string.IsNullOrEmpty(custom_id))
        {
            Debug.Log($"Logging in with PC Device");
            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
            {
                CustomId = custom_id,
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true
            }, response =>
            {
                Debug.Log($"Success Login with PC Device ID");
                ViewManager.Show<GameView>();
                playfabID = response.PlayFabId;
                playerName = playfabID;
                OnSignInSuccess.Invoke();
            }, error =>
            {
                Debug.Log($"Unsuccess Login with PC Device ID: {error.ErrorMessage}");
                OnSignInFailed.Invoke(error.ErrorMessage);
            });
        }
    }

    IEnumerator GameViewStart()
    {
        yield return new WaitForSeconds(1);
        ViewManager.Show<GameView>();
    }



    public void GetUserData(string key)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = playfabID,
            Keys = new List<string>()
            {
                key
            }
        }, response =>
        {
            Debug.Log($"Success GetUSerData");
            if (response.Data.ContainsKey(key))
            {
                OnUserDataRetrieved.Invoke(key, response.Data[key].Value);
            }
            else
            {
                OnUserDataRetrieved.Invoke(key, null);
            }
        }, error =>
        {
            Debug.Log($"Unsuccess GetUSerData: {error}");
        });

            
    }

    public void SetUserData(string key, string value)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {key, value }
            }
        }, response =>
        {
            Debug.Log($"Success SetUserData");
        }, error =>
         {
             Debug.Log($"Unsuccess SetUserData");

         });
    }

    public void GetStatistic(string key)
    {
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest()
        {
            StatisticNames = new List<string> { key }
        }, response =>
        {
            if (response.Statistics!=null  && response.Statistics.Count>0)
            {
                Debug.Log($"Successful GetStatistic");
                OnStatisticRetrieved.Invoke(key, response.Statistics[0].Value);
            }
            else { Debug.Log("Get null Statistic"); }
        }, error =>
        {
            Debug.Log($"Unsuccessful GetStatistic:{error.ErrorMessage}");
        });

    }

    public void SetStatistic(string key, int value)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>()
            {
                new StatisticUpdate()
                {
                    StatisticName = key,
                    Value = value
                }
            }
        }, response =>
         {
             Debug.Log($"Successful SetStatistic");
         }, error =>
         {
             Debug.Log($"Unsuccessful SetStatistic");
         }); 
    }


    public void GetLeaderboard(string key)
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest()
        {
            StatisticName = key,
            MaxResultsCount = 5
        }, response =>
        {
            if (response.Leaderboard != null)
            {
                Debug.Log($"Successful GetLeaderborad");
                OnLeaderboardRetrieved.Invoke(key, response.Leaderboard);
            }
            else { Debug.Log($"Get NULLLL Leaderborad"); }
            
        }, error =>
        {
            Debug.Log($"Unsuccessful GetLeaderborad");
        });
    }
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "LeaderBoardMap",
                    Value = score

                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);

    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult obj)
    {
        Debug.Log("Successfull leaderboard sent");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "LeaderBoardMap",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            newGo.GetComponent<UILeaderboardEntry>().placeText.text = (item.Position+1).ToString();
            newGo.GetComponent<UILeaderboardEntry>().nameText.text = item.DisplayName;//item.PlayFabId.ToString();
            newGo.GetComponent<UILeaderboardEntry>().valueText.text = item.StatValue.ToString();
            Debug.Log($"{item.Position} {item.PlayFabId} {item.StatValue}");
        }
    }

    public void SubmitName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = playerName,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult obj)
    {
        Debug.Log("Update display name!");
    }
}
