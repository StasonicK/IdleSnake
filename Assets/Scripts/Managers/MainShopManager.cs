using TMPro;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class MainShopManager : Singleton<MainShopManager>
    {
        [SerializeField]
        private TextMeshProUGUI[] _costs;

        [SerializeField]
        private TextMeshProUGUI[] _currentValue;

        [SerializeField]
        private TextMeshProUGUI[] _level;

        private static int _startCost=15;
        private static float _multiplyCost=1.2f;
        public static float newFoodPercent = 0.01f;
        private void Start()
        {
            int greatEyes = UpgradesManager.GreatEyes;
            int foodFinding = UpgradesManager.FoodFinding;
            int steelStomach = UpgradesManager.SteelStomach;
            int adrenalineGlands = BoostManager.AdrenalineGlands;
            int pathfinding = UpgradesManager.Pathfinding;
            int strongMuscles = UpgradesManager.StrongMuscles;
            int fastMetabolism = BoostManager.FastMetabolism;
            _costs[0].text = ((int)(_startCost * Mathf.Pow(_multiplyCost, greatEyes))).ToString();
            _costs[1].text = ((int)(_startCost *Mathf.Pow(_multiplyCost, foodFinding))).ToString();
            _costs[2].text = ((int)(_startCost * Mathf.Pow(_multiplyCost, steelStomach))).ToString();
            _costs[3].text = ((int)(_startCost * Mathf.Pow(_multiplyCost, adrenalineGlands))).ToString();
            _costs[4].text = ((int)(_startCost * Mathf.Pow(_multiplyCost, pathfinding))).ToString();
            _costs[5].text = ((int)(_startCost * Mathf.Pow(_multiplyCost, strongMuscles))).ToString();
            _costs[6].text = ((int)(_startCost * Mathf.Pow(_multiplyCost, fastMetabolism))).ToString();
            //_currentValue[0].text = greatEyes == 0 ? "0 sec" : (greatEyes == 1) ? (FieldManager.appleCooldown.ToString("f2")+" sec"): $"{FieldManager.appleCooldown - FieldManager.appleCooldown*Mathf.Pow(newFoodPercent, greatEyes-1) :f2} sec";
            _currentValue[1].text = $"{foodFinding}%";
            _currentValue[2].text = $"{1 +steelStomach}";
            _currentValue[3].text = $"{100 + 10 * (adrenalineGlands)}%";
            _currentValue[4].text = $"{6 + pathfinding }x{6 + pathfinding }";
            _currentValue[5].text = $"{1 + 0.1 * (strongMuscles - 1)}";
            _currentValue[6].text = $"{300 + 10 * (fastMetabolism)}%";
            _level[0].text = greatEyes.ToString()+" lvl";
            _level[1].text = foodFinding.ToString() + " lvl";
            _level[2].text = steelStomach.ToString() + " lvl";
            _level[3].text = adrenalineGlands.ToString() + " lvl";
            _level[4].text = pathfinding.ToString() + " lvl";
            _level[5].text = strongMuscles.ToString() + " lvl";
            _level[6].text = fastMetabolism.ToString() + " lvl";
        }
        public void BuyGreatEyes()
        {
            _currentValue[0].text =
                FieldManager.appleCooldown.ToString();
        }

        public void UpgradeGreatEyes()
        {
            int greatEyes = UpgradesManager.GreatEyes;
            int cost = (int)(_startCost *Mathf.Pow(_multiplyCost, greatEyes));
            if (UpgradesManager.AllCoins < cost) return;
            UIManager.Instance.UpdateCoinValue();
            UpgradesManager.AllCoins -= cost;
            UpgradesManager.GreatEyes = 1 + greatEyes;
            _costs[0].text = cost.ToString();
            _level[0].text = (greatEyes+1).ToString() + " lvl";
            FieldManager.Instance.UpgradeAppleCooldown();
            _currentValue[0].text = $"{FieldManager.appleCooldown:f2} sec";
        }

        public void UpgradeFoodFinding()
        {
            int foodFinding = UpgradesManager.FoodFinding;
            int cost = (int)(_startCost *Mathf.Pow(_multiplyCost, foodFinding));
            if (UpgradesManager.AllCoins < cost) return;
            UpgradesManager.AllCoins -= cost;
            UpgradesManager.FoodFinding = 1 + foodFinding;

            _currentValue[1].text = $"{1 +  foodFinding }%";
            _costs[1].text = cost.ToString();
            _level[1].text = (foodFinding+1).ToString() + " lvl";
            UIManager.Instance.UpdateCoinValue();
            FieldManager.Instance.UpgradeFoodFinding();
        }

        public void UpgradeSteelStomach()
        {
            int steelStomach = UpgradesManager.SteelStomach;
            int cost = (int)(_startCost * Mathf.Pow(_multiplyCost, steelStomach));
            if (UpgradesManager.AllCoins < cost) return;
            UpgradesManager.AllCoins -= cost;
            UpgradesManager.SteelStomach = 1 + steelStomach;
            _currentValue[2].text = $"{LevelGrowManager.baseGrowForFood + (steelStomach+1)*LevelGrowManager.upGrowForFood }";
            _costs[2].text = cost.ToString();
            _level[2].text = (steelStomach+1).ToString() + " lvl";
            UIManager.Instance.UpdateCoinValue();
            FieldManager.Instance.UpgradeSteelStomach();
        }

        public void UpgradeAdrenalineGlands()
        {
            int adrenalineGlands = BoostManager.AdrenalineGlands;
            int cost = (int)(_startCost* Mathf.Pow(_multiplyCost, adrenalineGlands));
            if (UpgradesManager.AllCoins < cost) return;
            UpgradesManager.AllCoins -= cost;
            BoostManager.AdrenalineGlands = adrenalineGlands + 1;
            UIManager.Instance.UpdateCoinValue();
            _currentValue[3].text = $"{100 + 10 * (adrenalineGlands+1)}%";
            _costs[3].text = cost.ToString();
            _level[3].text = (adrenalineGlands+1).ToString() + " lvl";
            FieldManager.Instance.UpgradeBoostSpeed();
        }

        public void UpgradePathfinding()
        {
            int pathfinding = UpgradesManager.Pathfinding;
            int cost =(int)(_startCost * Mathf.Pow(_multiplyCost, pathfinding));
            if (UpgradesManager.AllCoins < cost) return;
            UpgradesManager.AllCoins -= cost;
            UpgradesManager.Pathfinding += 1;
            _currentValue[4].text = $"{6 + pathfinding+1}x{6 + pathfinding+1}";
            _costs[4].text = cost.ToString();
            _level[4].text = (pathfinding+1).ToString() + " lvl";
            UIManager.Instance.UpdateCoinValue();
            FieldManager.Instance.ExpandField(FieldManager.Instance.FieldSize + 1);
        }

        public void UpgradeStrongMuscles()
        {
            int strongMuscles = UpgradesManager.StrongMuscles;
            int cost = (int)(_startCost *Mathf.Pow(_multiplyCost, strongMuscles));
            if (UpgradesManager.AllCoins < cost) return;
            UpgradesManager.AllCoins -= cost;
            UpgradesManager.StrongMuscles = strongMuscles + 1;
            _currentValue[5].text = $"{1 + 0.1 * (strongMuscles )}";
            _costs[5].text = cost.ToString();
            _level[5].text = (strongMuscles+1).ToString() + " lvl";
            UIManager.Instance.UpdateCoinValue();
            FieldManager.Instance.UpgradeStrongMuscles(strongMuscles+1);
        }

        public void UpgradeFastMetabolism()
        {
            int fastMetabolism = BoostManager.FastMetabolism;
            int cost = (int)(_startCost * Mathf.Pow(_multiplyCost, fastMetabolism));
            if (UpgradesManager.AllCoins < cost) return;
            UpgradesManager.AllCoins -= cost;
            BoostManager.FastMetabolism = fastMetabolism + 1;
            _currentValue[6].text = $"{300 + 10 * (fastMetabolism)}%";
            _costs[6].text = cost.ToString();
            _level[6].text = (fastMetabolism+1).ToString() + " lvl";
            UIManager.Instance.UpdateCoinValue();
            FieldManager.Instance.UpgradeBoostMetabolism();
        }
        
        
        public void LoadValue()
        {
            _startCost =(int) RemoteConfig.GetDouble("Evolution_Cost");
            _multiplyCost =(float)RemoteConfig.GetDouble("Evolution_Percent");
            FieldManager.appleCooldown =(float)RemoteConfig.GetDouble("Evolution_NewFood_Speed");
            newFoodPercent =(float)RemoteConfig.GetDouble("Evolution_NewFood_Percent")/100f;
            for(int i=0; i < UpgradesManager.GreatEyes - 1; i++)
            {
                FieldManager.Instance.UpgradeAppleCooldown();
            }
            _currentValue[0].text = FieldManager.appleCooldown.ToString("f2")+" sec";
        }
    }
}