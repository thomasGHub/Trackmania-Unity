using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;
    public static UnityEvent OnSignInSuccess = new UnityEvent();
    public static UnityEvent DownloadMapEnd = new UnityEvent();
    public static UnityEvent<string> OnSignInFailed = new UnityEvent<string>();
    public static UnityEvent OnRegisterSuccess = new UnityEvent();
    public static UnityEvent<string> OnRegisterFailed = new UnityEvent<string>();

    public static UnityEvent<string, string> OnUserDataRetrieved = new UnityEvent<string, string>();
    public static UnityEvent<string, int> OnStatisticRetrieved = new UnityEvent<string, int>();
    public static UnityEvent<string, List<PlayerLeaderboardEntry>> OnLeaderboardRetrieved = new UnityEvent<string, List<PlayerLeaderboardEntry>>();

    public string playfabID;
    public string entityID; //for json files
    public string entityType; //for json files
    public string playerName;
    public string playerPassword;

    private GameObject rowPrefab;
    private Transform rowsParent;

    private bool _isSignIn = false;
    private bool _isMapDownload = false;


    private void Awake()
    {
        instance = this;
        Debug.Log(Application.persistentDataPath);
    }

    private void Start()
    {
        //OnSignInSuccess.AddListener(()=> StartCoroutine(GameViewStart()));
        OnSignInSuccess.AddListener(SignInSucces);
        DownloadMapEnd.AddListener(MapDownloaded);
        OnRegisterSuccess.AddListener(()=>SubmitName(playerName)); 
        OnRegisterSuccess.AddListener(()=> SignIn(playerName, playerPassword));
        OnSignInFailed.AddListener(SignInFailedChangeView);

        StartCoroutine(AutoConnectAfter());

    }


    public void AutoConnect()
    {
        if (PlayerPrefs.HasKey("UserName") && PlayerPrefs.HasKey("Password"))
        {
            SignIn(PlayerPrefs.GetString("UserName"), PlayerPrefs.GetString("Password"));
        }
        else if (PlayerPrefs.HasKey("Custom_Id")  &&  PlayerPrefs.GetString("Custom_Id")== SystemInfo.deviceUniqueIdentifier)
        {
            SignInWithDevice();
        }
        else
        {
            ViewManager.Show<AuthentificationStartView>();
        }
    }

    public IEnumerator AutoConnectAfter()
    {
        yield return new WaitForSeconds(0.1f);
        AutoConnect();
    }



    public void SignInFailedChangeView(string error)
    {
        Debug.Log("errrororo");
        ViewManager.GetView<ConnectionFailedView>().Error.text = error;
        ViewManager.Show<ConnectionFailedView>();
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
                playerName = userName;
                playerPassword = password;
                OnRegisterSuccess.Invoke();
                
            },
            error =>
            {
                Debug.Log($"Unsuccesful Account Creation: {userName}, {emailAddresse}");
                OnRegisterFailed.Invoke(error.ErrorMessage);
            }

        );

    }

    public void SignIn(string userName, string password)
    {
        PlayFabClientAPI.LoginWithPlayFab(
            new LoginWithPlayFabRequest()
            {
                Username = userName,
                Password = password
            },
            response =>
            {
                Debug.Log($"Successful Account Login: {userName}");
                playfabID = response.PlayFabId;
                entityID = response.EntityToken.Entity.Id;
                entityType = response.EntityToken.Entity.Type;
                playerName = userName;
                PlayerPrefs.SetString("UserName", userName);
                PlayerPrefs.SetString("Password", password);
                OnSignInSuccess.Invoke();
            },
            error =>
            {
                Debug.Log($"Unsuccessful Account Login: {userName}");
                OnSignInFailed.Invoke(error.ErrorMessage);
            }
            );
    }


    public string GetRandomName(char key)
    {
        string[] Names = { "Rodrigo", "Mario", "Luigi", "Gustavo", "Gomez" };
        int number = (key) % Names.Length;
        PlayerPrefs.SetString("UserName", Names[number]);
        return Names[number];
    }


    public void SignInWithDevice()
    {
        string custom_id = SystemInfo.deviceUniqueIdentifier;

        if (!string.IsNullOrEmpty(custom_id))
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
                //Debug.Log($"Nice{response.PlayFabId}");
                playfabID = response.PlayFabId;
                entityID = response.EntityToken.Entity.Id;
                entityType = response.EntityToken.Entity.Type;
                playerName = response.PlayFabId;
                PlayerPrefs.SetString("Custom_Id", custom_id);
                SubmitName(GetRandomName(custom_id[custom_id.Length-1]));
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


   
    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    
    public void SubmitName(string _name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = _name,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);



    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult obj)
    {
        //Debug.Log("Update display name!");
        //SignIn(playerName, playerPassword);
    }

    private void SignInSucces()
    {
        _isSignIn = true;

        if (_isMapDownload)
            ChangeScene();
    }

    private void MapDownloaded()
    {
        _isMapDownload = true;

        if (_isSignIn)
            ChangeScene();
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("Offline", LoadSceneMode.Single);
    }



}
