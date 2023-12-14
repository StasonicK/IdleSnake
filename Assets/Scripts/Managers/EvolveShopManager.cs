using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
    public class EvolveShopManager : Singleton<EvolveShopManager>
    {
        // [SerializeField]
        // private GameObject _skillsPanel;

        [Serializable]
        public class Node
        {
            public int number;
            public int[] prev;
            public int[] block;
            public int costGrade;
            public GameObject openImageSkill;
            public Transform skillTransform;
            public Sprite skillSprite;
            public string skillDescription;
            public string skillName;
            public GameObject lockSkillImage;
            public GameObject[] lockedGrades;
            public GameObject[] openedGrades;
            public UnityEvent openAction;
        }

        public Node[] tree;

        public static EvolveGrades grades;

        [SerializeField]
        private TextMeshProUGUI _costText;


        [SerializeField]
        private GameObject _strokeOfMainSkill;

        [SerializeField]
        private Sprite _lockSprite;


        [SerializeField]
        private Image _mainSkillImage;

        [SerializeField]
        private TextMeshProUGUI _mainSkillDescription;

        [SerializeField]
        private TextMeshProUGUI _mainSkillName;


        [SerializeField]
        private GameObject _buyButton;

        [SerializeField]
        private GameObject _lockedBuyText;

        private static int chosenSkill;

        private void Start()
        {
            if (PlayerPrefs.HasKey("EvolveGrade"))
            {
                grades = JsonUtility.FromJson<EvolveGrades>(PlayerPrefs.GetString("EvolveGrade"));
            }

            Debug.Log(grades);
            grades ??= new EvolveGrades(tree.Length);

            UpdateViewTree();
            Instance.ChoseSkill(0);
        }

        private void UpdateViewTree()
        {
            foreach (var node in tree)
            {
                if (grades.isOpen[node.number])
                {
                    OpenSkill(node);
                    continue;
                }
                bool isPrevOpen = node.prev.Any(prev => grades.isOpen[prev]);
                bool isBlockOpen = node.block.Any(prev => grades.isOpen[prev]);
                bool isNeedOpen = isPrevOpen && !isBlockOpen;
                node.lockSkillImage.SetActive(!isNeedOpen);
                if (isBlockOpen)
                {
                    node.lockSkillImage.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f);

                }
            }
            tree[0].lockSkillImage.SetActive(false);
        }

        private void OpenSkill(Node skill)
        {
            foreach (GameObject lockedObject in skill.lockedGrades)
            {
                lockedObject.SetActive(false);
            }

            foreach (GameObject openedObject in skill.openedGrades)
            {
                openedObject.SetActive(true);
            }

            skill.openImageSkill.SetActive(true);
        }

        public void ChoseSkill(int i)
        {
            chosenSkill = i;
            _strokeOfMainSkill.transform.position = tree[i].skillTransform.position;
            _mainSkillImage.sprite = tree[i].skillSprite;
            _mainSkillDescription.text = tree[i].skillDescription;
            _mainSkillName.text = tree[i].skillName;

            if (grades.isOpen[i])
            {
                _buyButton.SetActive(false);
                _lockedBuyText.SetActive(false);
            }
            else if (i == 0 || (tree[i].prev.Any(prev => grades.isOpen[prev]) &&
                                !tree[i].block.Any(prev => grades.isOpen[prev])))
            {
                _buyButton.SetActive(true);
                _lockedBuyText.SetActive(false);
                _costText.text = tree[i].costGrade.ToString();
            }
            else if (tree[i].block.Any(prev => grades.isOpen[prev]))
            {
                _buyButton.SetActive(false);
                _lockedBuyText.SetActive(false);
                _mainSkillImage.sprite = _lockSprite;
                _mainSkillDescription.text = "???";
                _mainSkillName.text = "???";
            }
            else
            {
                _buyButton.SetActive(false);
                _lockedBuyText.SetActive(true);
                _mainSkillImage.sprite = _lockSprite;
                _mainSkillDescription.text = "???";
                _mainSkillName.text = "???";
            }
        }

        public void BuySkill()
        {
            if (PlayerData.EvolvePoint < tree[chosenSkill].costGrade) return;
            PlayerData.EvolvePoint -= tree[chosenSkill].costGrade;
            UIManager.Instance.UpdateEvolvePointValue();
            grades.isOpen[chosenSkill] = true;
            PlayerPrefs.SetString("EvolveGrade", JsonUtility.ToJson(grades));
            OpenSkill(tree[chosenSkill]);
            _buyButton.SetActive(false);
            UpdateViewTree();
            tree[chosenSkill].openAction.Invoke();
            AnalyticManager.Snake_Modification(tree[chosenSkill].skillName?.Replace(" ", ""));
        }

        public static void BuyGreatEyes()
        {
            UpgradesManager.GreatEyes = 1;
            MainShopManager.Instance.BuyGreatEyes();
            QuestManager.Instance.AddValueToQuest(QuestManager.Quest.QuestType.Unlock_Great_Eyes, 1);
        }

        public static void BuyFoodFinding()
        {
            UpgradesManager.FoodFinding = 1;
            FieldManager.Instance.UpgradeFoodFinding();
            QuestManager.Instance.AddValueToQuest(QuestManager.Quest.QuestType.Unlock_Food_Finding, 1);
        }


        public static void BuySteelStomach()
        {
            UpgradesManager.SteelStomach = 1;
            FieldManager.Instance.UpgradeSteelStomach();
        }


        public void BuyAdrenalineGlands()
        {
            BoostManager.AdrenalineGlands += 1;
            FieldManager.Instance.UpgradeBoostSpeed();
        }


        public void BuyFastMetabolism()
        {
            BoostManager.FastMetabolism += 1;
            FieldManager.Instance.UpgradeBoostMetabolism();
        }


        public static void BuyPathfinding()
        {
            UpgradesManager.Pathfinding += 1;
            AnalyticManager.Field_Expand();
            UIManager.Instance.changeShopEvent += FieldManager.Instance.ShowPopup;
            QuestManager.Instance.AddValueToQuest(QuestManager.Quest.QuestType.Unlock_Pathfinding, 1);
        }


        public static void BuyStrongMuscles()
        {
            UpgradesManager.StrongMuscles += 1;
            FieldManager.Instance.UpgradeStrongMuscles(UpgradesManager.StrongMuscles);
        }
    }

    public class EvolveGrades
    {
        public bool[] isOpen;

        public EvolveGrades(int lenght)
        {
            isOpen = new bool[lenght];
        }
    }
}