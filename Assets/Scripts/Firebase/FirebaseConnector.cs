using System.Collections;
using System;
using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using Firebase.Extensions;
using Firebase.Analytics;
//using Firebase.Auth;
using Utilities;
using Firebase;
//using Google.Play.Common;
//using Mycom.Tracker.Unity;

public class FirebaseConnector : Singleton<FirebaseConnector>
{
    public static string AuthCode => null;

    public static string UserId => null;
    //public static FirebaseUser FirebaseUser { get; private set; }

    public static bool IsFireBaseActive { get; private set; } = false;

    private static string _stateString;
    private static bool IsRemoteActive { get; set; } = false;
    private static bool IsAnalyticsActive { get; set; } = false;
    public static bool IsPlayGameActive { get; private set; } = false;
    public static bool IsFirebaseAuthActive { get; private set; } = false;

    public static bool IsGameVersionChecked { get; set; } = false;


    public void Awake()
    {
        RemoteConfig.OnConfigUpdate += Managers.BoostManager.LoadValues;
        RemoteConfig.OnConfigUpdate += Managers.MainShopManager.Instance.LoadValue;
        RemoteConfig.OnConfigUpdate += Managers.LevelGrowManager.LoadValue;
        LoadFirebase();
    }

    private static void LoadFirebase()
    {

        Debug.Log("Started Loading");
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
        }
        else
        {
            Debug.Log("Start firebase init.");
            _stateString = "Start firebase init.";
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                FirebaseAnalytics.LogEvent("Firebaseinit");
                try
                {
                    var fb = FirebaseApp.DefaultInstance;
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                    if (task.IsCompleted)
                    {
                        Debug.Log("Firebase init successfully");
                        _stateString = "Firebase init successfully";
                            //DynamicLinksManager.Instance.Init();
                            IsAnalyticsActive = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }

                Debug.Log("Start remote config init.");
                _stateString = "Start remote config init.";
                
            });
        }

    }
}