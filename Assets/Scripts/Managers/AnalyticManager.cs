using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;

namespace Managers
{

    public static class AnalyticManager
    {
        public static void SavePlayerMoney(int money)
        {
            FirebaseAnalytics.SetUserProperty("Coins", money.ToString());
        }

        #region Snake

        public static void Snake_Growth()
        {
            string eventString = string.Format("Snake_Growth");
            Debug.Log(eventString);
            FirebaseAnalytics.LogEvent(eventString);
        }

        public static void Snake_Growth_Size(int size)
        {
            string eventString = $"Snake_Growth_{size:d3}";
            Debug.Log(eventString);
            FirebaseAnalytics.LogEvent(eventString);
        }

        public static void Snake_Modification(string modification)
        {
            string eventString = string.Format("Snake_Modification");
            Debug.Log(eventString);
            FirebaseAnalytics.LogEvent(eventString, new Parameter("Modofocation", modification));
        }

        #endregion
        #region Field

        public static void Field_Expand()
        {
            string eventString = string.Format("Field_Expand");
            Debug.Log(eventString);
            FirebaseAnalytics.LogEvent(eventString);
        }

        #endregion
        #region Shop

        public static void Shop_Crystall(string Product)
        {
            string eventString = string.Format("Shop_Crystall", new Parameter("Product", Product));
            Debug.Log(eventString);
            FirebaseAnalytics.LogEvent(eventString);
        }

        public static void Shop_Money(string Product)
        {
            string eventString = string.Format("Shop_Money");
            Debug.Log(eventString);
            FirebaseAnalytics.LogEvent(eventString, new Parameter("Product", Product));
        }

        #endregion
        #region Level
        public static void Level_Start(int level)
        {
            string eventString = string.Format("Level_Start_{0:d4}", level);
            Debug.Log(eventString);
            FirebaseAnalytics.LogEvent(eventString);
        }

        public static void Level_Finish(int level)
        {
            string eventString = string.Format("Level_Finish_{0:d4}", level);
            Debug.Log(eventString);
            FirebaseAnalytics.LogEvent(eventString);
        }

        #endregion
        #region BP

        public static void BP_GameProgress(int level)
        {
            string eventString = string.Format("BP_GameProgress");
            Debug.Log(eventString);
            FirebaseAnalytics.LogEvent(eventString);
        }

        #endregion
    }
}