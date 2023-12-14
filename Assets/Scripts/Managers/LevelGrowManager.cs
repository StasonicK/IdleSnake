using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class LevelGrowManager : Utilities.Singleton<LevelGrowManager>
    {
        [SerializeField]
        private RectTransform _progressBar;

        [SerializeField]
        private TextEatFruit _pointPrefab;

        private static TextEatFruit pointPrefab;

        [SerializeField]
        private TextMeshProUGUI _progressText;

        private static TextMeshProUGUI progressText;

        [SerializeField]
        private Image _growButton;

        [SerializeField]
        private GameObject _newSkillPointPanel;

        [SerializeField]
        private GameObject _maxSizePanel;

        [SerializeField]
        private GameObject _shadowGrowButton;

        private static GameObject shadowGrowButton;

        [SerializeField]
        private TextMeshProUGUI _newLevelText;

        private static Image growButton;

        private static RectTransform progressBar;

        private static int currentLevel;
        private static int currentLevelState;
        private static int nextGrow;
        private static int _baseMultiplyGrow = 2;
        private static float _multiplyGrow = 1.5f;

        public static int baseGrowForFood = 1;
        public static int upGrowForFood = 1;
        private static float _startWight = 360;
        private static float percentMaxGrow=0.25f;

        public static int CurrentLevel
        {
            get => PlayerPrefs.GetInt("CurrentLevel", 2);
            private set
            {
                PlayerPrefs.SetInt("CurrentLevel", value);
                currentLevel = value;
            }
        }

        private static int CurrentLevelState
        {
            get => PlayerPrefs.GetInt("CurrentLevelState", 0);
            set
            {
                PlayerPrefs.SetInt("CurrentLevelState", value);
                currentLevelState = value;
            }
        }

        private void Start()
        {
            pointPrefab = _pointPrefab;
            shadowGrowButton = _shadowGrowButton;
            growButton = _growButton;
            currentLevel = CurrentLevel;
            currentLevelState = CurrentLevelState;
            progressBar = _progressBar;
            progressText = _progressText;
            nextGrow = 10 * Mathf.CeilToInt(Mathf.Pow(currentLevel * _baseMultiplyGrow, _multiplyGrow) / 10f);
            CheckGrowSnake();
            UpdateProgressBar();
        }

        public static void EatApple(int fruit, int id)
        {
            CurrentLevelState = (nextGrow * 1.5f) <= currentLevelState
                ? currentLevelState
                : (currentLevelState + baseGrowForFood * (1 + id));
            var text = Instantiate(pointPrefab, Snake.Instance.Head.position, new Quaternion());
            text._text.text = fruit.ToString();
            CheckGrowSnake();
            UpdateProgressBar();

            UpgradesManager.AllCoins += fruit;
            UIManager.Instance.UpdateCoinValue();
            Debug.Log($"Current Level {currentLevel}: {currentLevelState}/{nextGrow}");
        }

        private static void CheckGrowSnake()
        {
            if (currentLevelState < nextGrow) return;
            growButton.raycastTarget = true;
            growButton.color = Color.white;
            growButton.rectTransform.anchoredPosition = Vector3.zero;
            shadowGrowButton.SetActive(true);
        }

        public void NewGrowLevel()
        {
            _newLevelText.text = $"Now your Snake is <color=red>{currentLevel + 1}</color> meters long!";
            if (FieldManager.Instance.FieldSize * FieldManager.Instance.FieldSize * percentMaxGrow >=
                Snake.Instance.Segments.Count)
            {
                _newSkillPointPanel.SetActive(true);
            }
            else
            {
                _maxSizePanel.SetActive(true);
            }
        }

        public void CloseNewSkillPointPanel()
        {
            _newSkillPointPanel.SetActive(false);
            PlayerData.EvolvePoint += 1;
            CurrentLevelState = currentLevelState - nextGrow;
            if (currentLevel == 2)
            {
                TutorialManager.Instance.ShowEvolveHand();
            }
            CurrentLevel = currentLevel + 1;
            Snake.Instance.AddSegment();
            AnalyticManager.Snake_Growth();
            AnalyticManager.Snake_Growth_Size(currentLevel + 1);
            if (currentLevel == 16) QuestManager.Instance.AddValueToQuest(QuestManager.Quest.QuestType.Reach_16_Size, 1);
            growButton.raycastTarget = false;
            growButton.color = Color.black;
            shadowGrowButton.SetActive(false);
            growButton.rectTransform.anchoredPosition = Vector3.down * 5f;
            nextGrow = 10 * Mathf.CeilToInt(Mathf.Pow(currentLevel * _baseMultiplyGrow, _multiplyGrow) / 10f);
            Debug.Log("Next grow " + nextGrow);
            UpdateProgressBar();
            CheckGrowSnake();
            UIManager.Instance.UpdateEvolvePointValue();
            UIManager.Instance.UpdateSizeValue();
        }

        private static void UpdateProgressBar()
        {
            progressBar.offsetMax =
                new Vector2(
                    nextGrow <= currentLevelState
                        ? 0
                        : -_startWight + _startWight *  currentLevelState / nextGrow, progressBar.offsetMax.y);
            progressText.text = (nextGrow * 1.5f) <= currentLevelState ? "MAX" : $"{currentLevelState} / {nextGrow}";
        }

        public static void LoadValue()
        {
            _baseMultiplyGrow = (int) RemoteConfig.GetDouble("Snake_GrowthBase");
            _multiplyGrow = (float) RemoteConfig.GetDouble("Snake_GrowthMult");
            baseGrowForFood = (int) RemoteConfig.GetDouble("Food_Start");
            upGrowForFood = ((int) RemoteConfig.GetDouble("Food_Percent")) / 100;
            FieldManager.speedSnakeForTime = (float) RemoteConfig.GetDouble("Snake_Speed");
            FieldManager.percentMaxFood = (float) RemoteConfig.GetDouble("Field_MaxFoodPercent")/100;
            percentMaxGrow = (float) RemoteConfig.GetDouble("Field_MaxSnakePercent")/100;
        }

        public static void DebugGrowSnake()
        {
            if (!DebugController.isDebug) return;
            CurrentLevelState = currentLevelState + 100;
            UpdateProgressBar();
            CheckGrowSnake();
        }
    }
}