using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{

    public class TutorialManager : Utilities.Singleton<TutorialManager>
    {
        [SerializeField]
        private GameObject _tutorialHand;
        [SerializeField]
        private GameObject _tutorialBoostHand;
        [SerializeField]
        private GameObject _tutorialEvolveHand;

        private bool _isHandShowPrefs
        {
            get => PlayerPrefs.GetInt("IsTutorialHandShow", 0)==1;
            set => PlayerPrefs.SetInt("IsTutorialHandShow", value ? 1 : 0);
        }  
        private bool _isHandBoostShowPrefs
        {
            get => PlayerPrefs.GetInt("IsTutorialHandShow", 0)==1;
            set => PlayerPrefs.SetInt("IsTutorialHandShow", value ? 1 : 0);
        }  
        private bool _isHandEvolveShowPrefs
        {
            get => PlayerPrefs.GetInt("IsTutorialHandShow", 0)==1;
            set => PlayerPrefs.SetInt("IsTutorialHandShow", value ? 1 : 0);
        }

        private bool _isHandShow;
        private bool _isHandBoostShow;
        private bool _isHandEvolveShow;

        private void Start()
        {
            _isHandShow = _isHandShowPrefs;
            _isHandBoostShow = _isHandBoostShowPrefs;
            _isHandEvolveShow = _isHandEvolveShowPrefs;
            _tutorialHand.SetActive(!_isHandShow);
        }

        public void HideHand()
        {
            if (_isHandShow) return;
            _isHandShow = true;
            _isHandShowPrefs = _isHandShow;
            _tutorialHand.SetActive(false);
        }

        public void ShowBoostHand()
        {
            if (_isHandBoostShow) return;
            UIManager.Instance.changeShopEvent += ()=> _tutorialBoostHand.SetActive(true);
        }

        public void HideBoostHand()
        {
            if (_isHandBoostShow) return;
            _isHandBoostShow = true;
            _isHandBoostShowPrefs = _isHandBoostShow;
            _tutorialBoostHand.SetActive(false);
        }

        public void ShowEvolveHand()
        {
            if (_isHandEvolveShow) return;
            _tutorialEvolveHand.SetActive(true);
            UIManager.Instance.changeShopEvent +=  HideEvoleHand;
        }

        public void HideEvoleHand()
        {
            if (_isHandEvolveShow) return;
            _isHandEvolveShow = true;
            _isHandEvolveShowPrefs = _isHandEvolveShow;
            _tutorialEvolveHand.SetActive(false);
        }
    }
}
