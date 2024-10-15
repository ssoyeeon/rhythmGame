using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

namespace RhythmGame
{
    [System.Serializable]
    public class LevelData
    {
        [SerializeField]
        private int highScore;
        [SerializeField]
        private int clearCount;
        [SerializeField]
        private int playCount;
        private LevelObject levelObject;

        public LevelData(LevelObject _levelObject)
        {
            levelObject = _levelObject;
        }

        public int GetValue(string dataName)
        {
            switch (dataName)
            {
                case nameof(highScore):
                    return highScore;
                case nameof(clearCount):
                    return clearCount;
                case nameof(playCount):
                    return playCount;
                default:
                    return -1;
            }
        }

        public LevelObject GetLevelObject()
        {
            return levelObject;
        }
    }

    public class LevelManager : MonoBehaviour
    {
        public List<LevelObject> levelObjects;
        private Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();
        private List<string> levelKeys = new List<string>();

        public RectTransform levelItemsContainer;
        public GameObject levelItemPrefab;
        public float rotationRadius = 300f;
        public float rotationDuration = 0.5f;
        public float centerScaleFactor = 1.2f;
        public GameObject selectionIndicatorPrefab;

        private int currentSelectedIndex = 0;
        private List<RectTransform> levelItems = new List<RectTransform>();
        private List<GameObject> selectionIndicators = new List<GameObject>();

        public TextMeshProUGUI levelNameText;
        public TextMeshProUGUI levelInfoText;

        public static LevelManager Instance;

        private Coroutine rotationCoroutine;

        private bool isRotating = false;  // 회전 중인지 확인하는 변수 추가


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            if (levelObjects != null && levelObjects.Count != 0)
            {
                AddLevelDataToDictionary();
            }
        }

        private void Start()
        {
            CreateLevelItems();
            UpdateLevelDisplay();
        }

        private void CreateLevelItems()
        {
            float angleStep = 360f / levelKeys.Count;
            for (int i = 0; i < levelKeys.Count; i++)
            {
                GameObject levelItemGO = Instantiate(levelItemPrefab, levelItemsContainer);
                RectTransform levelItemRect = levelItemGO.GetComponent<RectTransform>();
                Image levelItemImage = levelItemGO.GetComponent<Image>();

                LevelData levelData = levels[levelKeys[i]];
                levelItemImage.sprite = levelData.GetLevelObject().LevelImage;

                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector2 position = new Vector2(Mathf.Sin(angle) * rotationRadius, -Mathf.Cos(angle) * rotationRadius);
                levelItemRect.anchoredPosition = position;

                levelItemRect.rotation = Quaternion.Euler(0, 0, -i * angleStep);

                levelItems.Add(levelItemRect);

                GameObject indicator = Instantiate(selectionIndicatorPrefab, levelItemRect);
                indicator.SetActive(false);
                selectionIndicators.Add(indicator);
            }

            UpdateItemsAppearance();
        }


        private void Update()
        {
            if (!isRotating)  // 회전 중이 아닐 때만 입력을 받음
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    RotateLevels(-1);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    RotateLevels(1);
                }
                else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                {
                    StartSelectedLevel();
                }
            }
        }

        private void RotateLevels(int direction)
        {
            if (isRotating) return;  // 이미 회전 중이면 새로운 회전을 시작하지 않음

            isRotating = true;
            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
            }

            currentSelectedIndex = (currentSelectedIndex + direction + levelKeys.Count) % levelKeys.Count;
            float targetRotation = -currentSelectedIndex * (360f / levelKeys.Count);

            rotationCoroutine = StartCoroutine(RotateCoroutine(targetRotation));
        }

        private IEnumerator RotateCoroutine(float targetRotation)
        {
            float startRotation = levelItemsContainer.rotation.eulerAngles.z;
            float elapsedTime = 0f;

            while (elapsedTime < rotationDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.SmoothStep(0, 1, elapsedTime / rotationDuration);
                float currentRotation = Mathf.LerpAngle(startRotation, targetRotation, t);

                levelItemsContainer.rotation = Quaternion.Euler(0, 0, currentRotation);
                UpdateItemsAppearance();

                yield return null;
            }

            levelItemsContainer.rotation = Quaternion.Euler(0, 0, targetRotation);
            UpdateLevelDisplay();
            UpdateItemsAppearance();

            isRotating = false;  // 회전 완료 후 상태 업데이트
        }

        private void UpdateItemsAppearance()
        {
            float angleStep = 360f / levelKeys.Count;
            for (int i = 0; i < levelItems.Count; i++)
            {
                int relativeIndex = (i - currentSelectedIndex + levelKeys.Count) % levelKeys.Count;
                float targetAngle = relativeIndex * angleStep;

                levelItems[i].rotation = Quaternion.Euler(0, 0, -targetAngle);

                if (relativeIndex == 0)
                {
                    levelItems[i].localScale = Vector3.one * centerScaleFactor;
                    selectionIndicators[i].SetActive(true);
                    levelItems[i].SetAsLastSibling();
                }
                else
                {
                    levelItems[i].localScale = Vector3.one;
                    selectionIndicators[i].SetActive(false);
                }

                float normalizedDistance = Mathf.Abs(relativeIndex) / (levelKeys.Count / 2f);
                float alpha = 1 - normalizedDistance * 0.5f;
                Color itemColor = levelItems[i].GetComponent<Image>().color;
                itemColor.a = alpha;
                levelItems[i].GetComponent<Image>().color = itemColor;
            }
        }

        private void UpdateLevelDisplay()
        {
            if (levelKeys.Count == 0) return;

            string levelKey = levelKeys[currentSelectedIndex];
            LevelData levelData = levels[levelKey];
            LevelObject levelObject = levelData.GetLevelObject();

            if (levelNameText != null)
                levelNameText.text = levelObject.levelName;
            if (levelInfoText != null)
                levelInfoText.text = $"최고 점수: {levelData.GetValue("highScore")}\n" +
                                     $"클리어 횟수: {levelData.GetValue("clearCount")}\n" +
                                     $"플레이 횟수: {levelData.GetValue("playCount")}";
        }

        public void StartSelectedLevel()
        {
            if (levelKeys.Count > 0)
            {
                StartLevel(levels[levelKeys[currentSelectedIndex]]);
            }
        }

        public void StartLevel(LevelData levelData)
        {
            Debug.Log("게임 시작! " + levelData.GetLevelObject().levelName);
            GameManager.Instance.SetLevelData(levelData);
            SceneManager.LoadScene("GameScene");
        }

        private void AddLevelDataToDictionary()
        {
            foreach (var levelObject in levelObjects)
            {
                string levelKey = levelObject.levelName + "_" + levelObject.difficulty.ToString();

                if (!levels.ContainsKey(levelKey))
                {
                    LevelData levelData = new LevelData(levelObject);
                    levels.Add(levelKey, levelData);
                    levelKeys.Add(levelKey);
                    Debug.Log($"{levelObject.levelName}이(가) 딕셔너리에 추가되었습니다.");
                }
            }
        }

        public LevelData GetLevelDataFromDictionary(string levelKey, string levelName = null, int levelDifficulty = -1)
        {
            if (levelKey == null)
            {
                if (levelName == null || levelDifficulty == -1)
                {
                    return null;
                }

                levelKey = levelName + "_" + levelDifficulty.ToString();
            }

            if (levelKeys.Contains(levelKey))
            {
                return levels[levelKey];
            }

            return null;
        }
    }
}