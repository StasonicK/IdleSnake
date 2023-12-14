using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class FieldManager : Singleton<FieldManager>
    {
        [Header("Popups"), SerializeField]
        private GameObject _expandFieldPopup;

        [Header("Field obj"), SerializeField]
        private Transform _fieldParent;

        [SerializeField]
        private GameObject _cellPrefab;

        [Header("Apple Properties"), SerializeField]
        private GameObject _applePrefab;

        [SerializeField]
        private GameObject _pineapplePrefab;

        [SerializeField]
        private GameObject _appleParent;

        public static float appleCooldown = 10;

        [SerializeField]
        private float _appleMax;

        [Header("Properties"), SerializeField]
        private float _cellSize;

        [SerializeField]
        private int _fieldSize;

        public int FieldSize => _fieldSize;

        [SerializeField]
        private List<Color> _colors = new();

        private int _currentColor;

        private List<List<Cell>> _cellList = new();

        private List<Vector2Int> _cellSnake = new();

        private readonly List<Apple> _appleList = new();

        private int _foodFinding;

        private int _pointForFood;

        public static float speedSnakeForTime = 0.5f;

        private int _speedBoost;

        private int _metabolismBoost;

        private Camera _mainCamera;

        public static float percentMaxFood = 0.25f;



        private void Start()
        {
            _mainCamera = Camera.main;
            InitializeField();
            InitializeSnakeOnField();
            _foodFinding = UpgradesManager.FoodFinding;
            _pointForFood = UpgradesManager.SteelStomach;
            StartCoroutine(MovementProcess());
            StartCoroutine(AppleSpawnProcess());
            ExpandField(FieldSize + UpgradesManager.Pathfinding);
            _speedBoost = BoostManager.AdrenalineGlands;
            _metabolismBoost = BoostManager.FastMetabolism;
            for (int i = 0; i < UpgradesManager.StrongMuscles; i++)
            {
                UpgradeStrongMuscles(i);
            }
        }

        private void InitializeField()
        {
            _currentColor = 0;
            _cellList = new List<List<Cell>>();
            for (int i = 0; i < _fieldParent.childCount; i++)
            {
                Destroy(_fieldParent.GetChild(0).gameObject);
            }

            float startPosition = -(float)_fieldSize / 2 * _cellSize;

            for (int i = _fieldSize - 1; i >= 0; i--)
            {
                _cellList.Add(new List<Cell>());
                if (_fieldSize % 2 == 0) _currentColor = (_colors.Count - 1 == _currentColor) ? 0 : _currentColor + 1;
                for (int j = _fieldSize - 1; j >= 0; j--)
                {
                    GameObject cellObject = Instantiate(_cellPrefab, _fieldParent, true);
                    cellObject.transform.position = new Vector3(startPosition + (_fieldSize - 1 - i + 0.5f) * _cellSize,
                        startPosition + (_fieldSize - 1 - j + 0.5f) * (_cellSize), 1);
                    cellObject.name = $"Cell[{i}][{j}]";
                    Cell cell = new Cell
                    {
                        cellSprite = cellObject.GetComponent<SpriteRenderer>()
                    };
                    cell.cellSprite.color = _colors[_currentColor];
                    _currentColor = (_colors.Count - 1 == _currentColor) ? 0 : _currentColor + 1;
                    cell.cellObject = cellObject;
                    _cellList[_fieldSize - 1 - i].Add(cell);
                }
            }
        }

        private void InitializeSnakeOnField()
        {
            _cellSnake = new List<Vector2Int>();
            Vector2Int startId = new Vector2Int(_fieldSize / 2, _fieldSize / 2);
            Snake.Instance.Head.position = _cellList[startId.x][startId.y].cellObject.transform.position;
            _cellSnake.Add(startId);
            Vector2Int direction = PathFinder.RandomDirection();
            Vector2Int target = -direction;
            Vector2Int nextCell = startId + direction;
            startId = nextCell;


            foreach (var segment in Snake.Instance.Segments)
            {
                segment.segmentTransform.position =
                    _cellList[startId.x][startId.y].cellObject.transform.position;
                _cellSnake.Add(startId);
                while (_cellSnake.Contains(nextCell))
                {
                    direction = PathFinder.RandomDirection();
                    nextCell = new Vector2Int(Mathf.Clamp(startId.x + direction.x, 0, _fieldSize - 1),
                        Mathf.Clamp(startId.y + direction.y, 0, _fieldSize - 1));
                }

                startId = nextCell;
            }

            startId = _cellSnake[0];
            Snake.Instance.UpdateSnakeSprites(_cellList[startId.x + target.x][startId.y + target.y].cellObject.transform
                .position);
            Snake.Instance.UpdateSnakeSprites(_cellList[startId.x + target.x][startId.y + target.y].cellObject.transform
                .position);
        }

        public void MoveSnakeDirection(Vector2Int direction)
        {
            Vector2Int nextCell = _cellSnake[0] + direction;
            nextCell = new Vector2Int(Mathf.Clamp(nextCell.x, 0, _fieldSize - 1),
                Mathf.Clamp(nextCell.y, 0, _fieldSize - 1));
            if (nextCell == _cellSnake[0] || nextCell == _cellSnake[1]) return;
            Snake.Instance.MoveToTarget(_cellList[nextCell.x][nextCell.y].cellObject.transform.position);
            _cellSnake.Insert(0, nextCell);
            _cellSnake.RemoveAt(_cellSnake.Count - 1);
        }

        public void MoveSnakeId(Vector2Int cell)
        {
            if (cell == _cellSnake[0] || cell == _cellSnake[1]) return;
            Snake.Instance.MoveToTarget(_cellList[cell.x][cell.y].cellObject.transform.position);
            _cellSnake.Insert(0, cell);
            _cellSnake.RemoveAt(_cellSnake.Count - 1);
        }

        public void ShowPopup()
        {
            _expandFieldPopup.SetActive(true);
        }

        public void OnCloseExpandPopup()
        {
            _expandFieldPopup.SetActive(false);
            ExpandField(_fieldSize+1);
        }

        private void MoveSnakeRandom()
        {
            Vector2Int direction = PathFinder.RandomDirection();
            Vector2Int nextCell = _cellSnake[0] + direction;
            nextCell = new Vector2Int(Mathf.Clamp(nextCell.x, 0, _fieldSize - 1),
                Mathf.Clamp(nextCell.y, 0, _fieldSize - 1));
            while (nextCell == _cellSnake[0] || nextCell == _cellSnake[1])
            {
                direction = PathFinder.RandomDirection();
                nextCell = new Vector2Int(Mathf.Clamp(_cellSnake[0].x + direction.x, 0, _fieldSize - 1),
                    Mathf.Clamp(_cellSnake[0].y + direction.y, 0, _fieldSize - 1));
            }

            Snake.Instance.MoveToTarget(_cellList[nextCell.x][nextCell.y].cellObject.transform.position);

            Apple foundApple = _appleList.Find(x => x.appleId == nextCell);

            if (foundApple != null)
            {
                Destroy(foundApple.appleObject);
                _appleList.Remove(foundApple);
                Debug.Log($"Removed apple! Apple list count :{_appleList.Count}");
                UIManager.Instance.UpdateFoodValue($"{_appleList.Count}/{Mathf.Floor(_appleMax - 1)}");
                LevelGrowManager.EatApple(
                    (int)((foundApple.point * LevelGrowManager.baseGrowForFood +
                            LevelGrowManager.upGrowForFood * _pointForFood) *
                           (BoostManager.isBoostMetabolism
                               ? (3 + (_metabolismBoost - 1) * 0.1f)
                               : 1)), foundApple.fruitID);
            }

            _cellSnake.Insert(0, nextCell);
            _cellSnake.RemoveAt(_cellSnake.Count - 1);
        }

        private void SpawnRandomApple()
        {
            if (_appleList.Count >= Mathf.Floor(_appleMax - 1)) return;
            Vector2Int id = new Vector2Int(Random.Range(0, _fieldSize), Random.Range(0, _fieldSize));
            while (_cellSnake.IndexOf(id) != -1 || _appleList.Find(x => x.appleId == id) != null)
                id = new Vector2Int(Random.Range(0, _fieldSize), Random.Range(0, _fieldSize));
            SpawnApple(id);
            UIManager.Instance.UpdateFoodValue($"{_appleList.Count}/{Mathf.Floor(_appleMax - 1)}");
        }

        private void SpawnApple(Vector2Int id)
        {
            bool isPineapple = Random.Range(0, 1.0f) < _foodFinding * 0.01f;
            GameObject obj = Instantiate(!isPineapple ? _applePrefab : _pineapplePrefab, _appleParent.transform, true);
            Apple apple = new Apple
            {
                appleObject = obj,
                point = isPineapple ? 10 : 1,
                fruitID = isPineapple ? 1 : 0
            };
            obj.transform.position = (Vector2)_cellList[id.x][id.y].cellObject.transform.position;
            apple.appleId = id;
            _appleList.Add(apple);
        }

        private IEnumerator MovementProcess()
        {
            while (true)
            {
                if (Snake.Instance.Segments.Count < LevelGrowManager.CurrentLevel) Snake.Instance.AddSegment();
                MoveSnakeRandom();
                yield return new WaitForSecondsRealtime(speedSnakeForTime /
                                                        (BoostManager.isBoostSpeed ? 2 + (_speedBoost - 1) * 0.1f : 1));
            }
        }

        private IEnumerator AppleSpawnProcess()
        {
            while (true)
            {
                if (UpgradesManager.GreatEyes > 0 && _appleList.Count < _appleMax - 1) SpawnRandomApple();
                yield return new WaitForSecondsRealtime(appleCooldown);
            }
        }

        public void ExpandField(int newSize)
        {
            _fieldSize = newSize;
            _mainCamera.orthographicSize = 3.3f + (_fieldSize - 6) * 0.5f;
            _mainCamera.transform.position = new Vector3(0, -1 - (_fieldSize - 6) * 0.15f, -10);
            float startPosition = -_fieldSize / 2f * _cellSize;
            _appleMax = newSize * newSize * percentMaxFood;
            Debug.Log("apple max " + _appleMax);
            UIManager.Instance.UpdateFoodValue($"{_appleList.Count}/{Mathf.Floor(_appleMax - 1)}");
            for (int i = _fieldSize - 1; i >= 0; i--)
            {
                if (_cellList.Count < _fieldSize) _cellList.Add(new List<Cell>());
                if (_fieldSize % 2 == 0) _currentColor = (_colors.Count - 1 == _currentColor) ? 0 : _currentColor + 1;
                for (int j = _fieldSize - 1; j >= 0; j--)
                {
                    if (_cellList[_fieldSize - 1 - i].Count < _fieldSize)
                    {
                        GameObject cellObject = Instantiate(_cellPrefab, _fieldParent, true);
                        Cell cell = new Cell
                        {
                            cellSprite = cellObject.GetComponent<SpriteRenderer>(),
                            cellObject = cellObject
                        };
                        _cellList[_fieldSize - 1 - i].Add(cell);
                    }

                    _cellList[_fieldSize - 1 - i][_fieldSize - 1 - j].cellSprite.color = _colors[_currentColor];
                    _currentColor = (_colors.Count - 1 == _currentColor) ? 0 : _currentColor + 1;
                    _cellList[_fieldSize - 1 - i][_fieldSize - 1 - j].cellObject.transform.position = new Vector3(
                        startPosition + (i + 0.5f) * _cellSize,
                        startPosition + (j + 0.5f) * (_cellSize), 1);
                    _cellList[_fieldSize - 1 - i][_fieldSize - 1 - j].cellObject.name = $"Cell[{i}][{j}]";
                }
            }

            foreach (Apple apple in _appleList)
            {
                apple.appleObject.transform.position =
                    (Vector2)_cellList[apple.appleId.x][apple.appleId.y].cellObject.transform.position;
            }

            Snake.Instance.Head.position = _cellList[_cellSnake[0].x][_cellSnake[0].y].cellObject.transform.position;

            for (int i = 1; i < _cellSnake.Count; i++)
            {
                Snake.Instance.Segments[i - 1].segmentTransform.position =
                    _cellList[_cellSnake[i].x][_cellSnake[i].y].cellObject.transform.position;
            }
        }

        public void UpgradeAppleCooldown()
        {
            appleCooldown -= appleCooldown * MainShopManager.newFoodPercent;
        }

        public void UpgradeFoodFinding()
        {
            _foodFinding += 1;
        }

        public void UpgradeSteelStomach()
        {
            _pointForFood += 1;
        }

        public void UpgradeBoostSpeed()
        {
            _speedBoost += 1;
        }

        public void UpgradeBoostMetabolism()
        {
            _metabolismBoost += 1;
        }

        public void UpgradeStrongMuscles(int grade)
        {
            if (grade == 1)
            {
                speedSnakeForTime -= 0.05f;
            }
            else
            {
                speedSnakeForTime -= 0.005f;
            }
        }

        private class Cell
        {
            public GameObject cellObject;
            public SpriteRenderer cellSprite;
        }

        private class Apple
        {
            public GameObject appleObject;
            public Vector2Int appleId;
            public int point;
            public int fruitID;
        }

        public void ExpandSnakeList()
        {
            Vector2Int lastCell = (_cellSnake.Count > 0) ? _cellSnake[^1] : Vector2Int.zero;
            _cellSnake.Add(lastCell);
        }
    }
}