using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField]
        private TextMeshProUGUI _coinsValue;
        [SerializeField]
        private TextMeshProUGUI _diamondValue;

        [SerializeField]
        private TextMeshProUGUI _sizeValue;

        [SerializeField]
        private TextMeshProUGUI _ePValue;
        [SerializeField]
        private TextMeshProUGUI _ePValueInSkillTreeScreen;

        [SerializeField]
        private TextMeshProUGUI _maxFood;
        [SerializeField]
        private GameObject _firstMenu;
        [SerializeField]
        private GameObject _secondMenu;
        [SerializeField]
        private TextMeshProUGUI _version;

        public UnityAction changeShopEvent; 
        private void Start()
        {
            _version.text = Application.version;
            UpdateCoinValue();
            UpdateEvolvePointValue();
            UpdateDiamondValue();
            UpdateSizeValue();
        }

        public void UpdateCoinValue()
        {
            _coinsValue.text = UpgradesManager.AllCoins.ToString();
        }
        public void UpdateFoodValue(string text)
        {
            _maxFood.text = text;
        }
        
        public void UpdateEvolvePointValue()
        {
            _ePValue.text = PlayerData.EvolvePoint.ToString();
            _ePValueInSkillTreeScreen.text = PlayerData.EvolvePoint.ToString();
        }  
        public void UpdateDiamondValue()
        {
            _diamondValue.text = PlayerData.diamond.ToString();
        }
        
        public void UpdateSizeValue()
        {
            _sizeValue.text = LevelGrowManager.CurrentLevel.ToString();
        }

        public void ChangeMenuAnim(Animator animator)
        {
            animator.SetBool("isLeft", !animator.GetBool("isLeft"));
            _firstMenu.SetActive(!_firstMenu.activeSelf);
            _secondMenu.SetActive(!_secondMenu.activeSelf);
            changeShopEvent?.Invoke();
            changeShopEvent = null;
           
        }

        public void DebugAddEvolvePoint()
        {
            if (!DebugController.isDebug) return;
            PlayerData.EvolvePoint += 100;
            UpdateEvolvePointValue();
        }
        public void DebugAddCoin()
        {
            if (!DebugController.isDebug) return;
            UpgradesManager.AllCoins += 100;
            UpdateCoinValue();
        }

        public void ResetAll()
        {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }
    }
}
