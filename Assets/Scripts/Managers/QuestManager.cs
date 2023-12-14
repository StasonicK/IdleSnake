using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Managers
{

    public class QuestManager : Utilities.Singleton<QuestManager>
    {
        [Serializable]
        public class Quest
        {
            public enum QuestType
            {
                Unlock_Great_Eyes,
                Unlock_Food_Finding,
                Unlock_Pathfinding,
                Reach_16_Size,
                Unlock_New_Level
            }

            [Header("Quest info")]
            public QuestType Type;

            public string QuestDesc;

            private int _questCurrentValue = -1;

            public int QuestCurrentValue
            {

                get
                {
                    if (_questCurrentValue == -1) _questCurrentValue = PlayerPrefs.GetInt(Type + "Value", 0);
                    return _questCurrentValue;
                }

                set
                {
                    _questCurrentValue = value;
                    PlayerPrefs.SetInt(Type + "Value", value);
                }
            }

            public int QuestGoalValue;

            public List<QuestType> PrevType = new List<QuestType>();

            [Header("Quest object")]
            public GameObject QuestGameObject;

            public TextMeshProUGUI QuestDescText;

            public TextMeshProUGUI QuestProgressText;

            public Image[] Light;

            private bool isStarted = false;
            private bool isCompleted = false;
            public void StartQuest()
            {
                if (isStarted) return;
                foreach (var light in Light)
                {
                    light.enabled = true;
                }
                isStarted = true;
            } 
            public void CompleteQuest()
            {
                if (isCompleted) return;
                foreach (var light in Light)
                {
                    light.enabled =  false;
                }
                isCompleted = true;
            }
            public bool IsComplete()
            {
                return QuestCurrentValue == QuestGoalValue;
            }


            public bool IsAvaible()
            {
                foreach (Quest.QuestType type in PrevType) if (!QuestManager.Instance.IsQuestTypeComplete(type)) return false;
                return true;
            }
        }

        public bool IsNotAvaibleShow
        {
            get => PlayerPrefs.GetInt("IsNotAvailableQuestShowPopup", 0) == 1;
            set => PlayerPrefs.SetInt("IsNotAvailableQuestShowPopup", value ? 1 : 0);
        }

        [SerializeField]
        private List<Quest> _questList = new List<Quest>();

        [SerializeField]
        private GameObject _noAvaibleQuestInList;

        [SerializeField]
        private GameObject _noAvaibleQuestPopup;

        [Header("Current quest main"), SerializeField]
        private GameObject _currentQuestObject;

        [SerializeField]
        private TextMeshProUGUI _currentQuestDesc;

        [SerializeField]
        private TextMeshProUGUI _currentQuestProgress;

        private Dictionary<Quest.QuestType, Quest> _questDict = new Dictionary<Quest.QuestType, Quest>();

        private void Awake()
        {
            for (int i = 0; i < _questList.Count; i++) _questDict.Add(_questList[i].Type, _questList[i]);
            UpdateCurrentQuestUI();
        }

        private bool IsQuestTypeAvaible(Quest.QuestType questType)
        {
            Quest currentQuest = _questDict[questType];
            foreach (Quest.QuestType type in currentQuest.PrevType) if (!IsQuestTypeComplete(type)) return false;
            return true;
        }


        public bool IsQuestTypeComplete(Quest.QuestType questType)
        {
            return _questDict[questType].QuestCurrentValue == _questDict[questType].QuestGoalValue;
        }

     

        public void UpdateQuestListUI()
        {
            _noAvaibleQuestInList.SetActive(true);
            foreach (Quest quest in _questDict.Values)
            {
                bool IsEnable = quest.IsAvaible();
                quest.QuestGameObject.SetActive(IsEnable);
                if (IsEnable)
                {
                    _noAvaibleQuestInList.SetActive(false);
                    quest.QuestDescText.text = quest.QuestDesc;
                    quest.QuestProgressText.text = $"{quest.QuestCurrentValue}/{quest.QuestGoalValue}";
                    if (quest.IsComplete()) { 
                        quest.QuestDescText.fontStyle = FontStyles.Strikethrough;
                    }
                    IsNotAvaibleShow = false;
                }

            }
        }

        public void UpdateCurrentQuestUI()
        {
            _currentQuestObject.SetActive(false);
            foreach (Quest quest in _questDict.Values)
            {
                bool IsEnable = quest.IsAvaible() && !quest.IsComplete();
                if (IsEnable)
                {
                    quest.StartQuest();
                    _currentQuestObject.SetActive(true);
                    _currentQuestDesc.text = quest.QuestDesc;
                    _currentQuestProgress.text = $"{quest.QuestCurrentValue}/{quest.QuestGoalValue}";
                    IsNotAvaibleShow = false;
                    return;
                }
                if (quest.IsComplete())
                {
                    quest.CompleteQuest();
                }

            }

            if (!IsNotAvaibleShow)
            {
                IsNotAvaibleShow = true;
                _noAvaibleQuestPopup.SetActive(true);
            }
        }

        public void AddValueToQuest(Quest.QuestType questType, int value, bool isForce = false)
        {
            if (isForce || (IsQuestTypeAvaible(questType) && !IsQuestTypeComplete(questType))) _questDict[questType].QuestCurrentValue += value;
            UpdateCurrentQuestUI();
            /*   if (IsComplete(_questDict[questType]){
                   bool flagAll = true;
                   foreach(Quest quest in _questDict.Values)
                   {
                       flagAll = flagAll && IsComplete(quest);
                   }
                   if (flagAll)
                   {
                       _currentQuestObject.SetActive(false);

                   }
               }*/


        }
    }
}