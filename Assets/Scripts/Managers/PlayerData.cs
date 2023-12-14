using UnityEngine;

namespace Managers
{
    public static class PlayerData
    {
        private static int countRequests = 0;
        public static int EvolvePoint
        {
            get { 
                if(countRequests == 0)
                {
                    countRequests = 1;
                    evolvePoint = PlayerPrefs.GetInt("EvolvePoint");
                }
                return evolvePoint;
            }
            set
            {
                PlayerPrefs.SetInt("EvolvePoint", value);
                evolvePoint = value;
            }
        }
        private static int evolvePoint;
        public static int diamond;
    }
}
