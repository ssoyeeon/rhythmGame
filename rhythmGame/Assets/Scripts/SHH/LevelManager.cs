using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RhythmGame
{
    /// <summary>
    /// 레벨의 데이터를 저장함
    /// </summary>
    [System.Serializable]
    public class LevelData
    {
        [SerializeField]
        private int highScore;                  // 최고 점수
        [SerializeField]
        private int clearCount;                 // 클리어 횟수
        [SerializeField]
        private int playCount;                  // 플레이 횟수
        private LevelObject levelObject;        // 이 레벨의 데이터
        
        public LevelData(LevelObject _levelObject)
        {
            levelObject = _levelObject;
        }
        /// <summary>
        /// 하이스코어, 클리어카운트, 플레이 카운트 받음
        /// </summary>
        /// <param name="dataName">highScore, clearCount, playCount</param>
        /// <returns>해당하는 값을 반환</returns>
        public int GetValue(string dataName)
        {
            switch(dataName)
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
        /// <summary>
        /// 노트 데이터를 반환함
        /// </summary>
        /// <returns>LevelObject를 반환함</returns>
        public LevelObject GetLevelObject()
        {
            return levelObject;
        }
    }

    public class LevelManager : MonoBehaviour
    {
        public List<LevelObject> levelObjects;              // 게임의 레벨들을 담을 리스트
        private Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();      // 레벨들
        private List<string> levelKeys = new List<string>();
        public Button[] Buttons;                            // 추가하려면 여기에
        private List<Button> levelButtons = new List<Button>();          // 레벨 버튼들
        private int levelIndex;
        private int levelCount;

        void Awake()
        {
            if (levelObjects.Count != 0 || levelObjects != null)
            {
                AddLevelDataToDictionary();
            }

            SetLevelButtons(Buttons);
        }
        /// <summary>
        /// 버튼을 세팅하는 함수
        /// 버튼의 수가 레벨의 수보다 많을 경우 이후 버튼들은 비활성화 함
        /// </summary>
        /// <param name="buttons">레벨을 불러올 버튼들</param>
        public void SetLevelButtons(Button[] buttons)
        {
            if(levelButtons.Count == 0 || levelButtons == null)
            {
                foreach (var button in buttons)
                {
                    levelButtons.Add(button);
                }
            }
            else
            {
                foreach (var button in buttons)
                {
                    if(!levelButtons.Contains(button))
                    {
                        levelButtons.Add(button);
                    }
                }
            }

            for(int i = 0; i < levelButtons.Count; i ++)
            {
                if(levels.Count <= i)
                {
                    levelButtons[i].gameObject.SetActive(false);
                }
                else
                {
                    levelButtons[i].transform.GetChild(0).GetComponent<Text>().text = levels[levelKeys[i]].GetLevelObject().levelName;
                    levelButtons[i].onClick.AddListener(() =>StartLevel(levels[levelKeys[i]]));

                    if(levels[levelKeys[i]].GetLevelObject().LevelImage)
                    {
                        levelButtons[i].image.sprite = levels[levelKeys[i]].GetLevelObject().LevelImage;
                    }
                }

            }
        }
        public void StartLevel(LevelData levelData)
        {
            Debug.Log("게임 시작!");
            // 추가예정
        }
        /// <summary>
        /// 레벨데이터를 딕셔너리에서 찾아옴
        /// </summary>
        /// <param name="levelKey">주로 사용함 키는 키 리스트에도 있음</param>
        /// <param name="levelName">레벨의 이름과 난이도로 찾을때 씀</param>
        /// <param name="levelDifficulty">레벨의 이름과 난이도로 찾을때 씀</param>
        /// <returns>딕셔너리에서 레벨 데이터를 반환함</returns>
        public LevelData GetLevelDataFromDictionary(string levelKey, string levelName = null, int levelDifficulty = -1)
        {
            if(levelKey == null)
            {
                if(levelName == null || levelDifficulty == -1)
                {
                    return null;
                }

                levelKey = levelName + "_" + levelDifficulty.ToString();
            }

            if(levelKeys.Contains(levelKey))
            {
                return levels[levelKey];
            }

            return null;
        }

        /// <summary>
        /// 레벨 데이터들을 딕셔너리에 넣음
        /// 처음 한번만 실행하는 함수임
        /// </summary>

        private void AddLevelDataToDictionary()
        {
            if (levelObjects.Count == 0)
            {
                return;
            }

            for (int i = 0; i < levelObjects.Count; i++)
            {
                string levelKey = levelObjects[i].levelName.ToString() + "_" + levelObjects[i].difficulty.ToString();

                if (!levels.ContainsKey(levelKey))
                {
                    LevelData levelData = new LevelData(levelObjects[i]);
                    levels.Add(levelKey, levelData);
                    levelKeys.Add(levelKey);
                    Debug.Log(levelData.GetLevelObject().levelName.ToString() + "is Added in Dictionary");
                }
            }

            levelCount = levels.Count;
        }
        /// <summary>
        /// 인덱스를 통해 호출할때 사용함
        /// 인덱스라 함은 레벨의 순서대로 1234 돌아감 인덱스는 이 클래스 내에 있는 인덱스임
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns>딕셔너리에 있는 레벨 데이터를 반환해옴</returns>
        private LevelData GetLevelDataByIndex(Enums.Modifier modifier)
        {
            if(modifier != Enums.Modifier.NONE)
            {
                if (modifier == Enums.Modifier.POSITIVE)
                {
                    levelIndex++;

                    if (levelIndex >= levelCount)
                    {
                        levelIndex = 0;
                    }
                }
                else
                {
                    levelIndex--;

                    if(levelIndex < 0)
                    {
                        levelIndex = levelCount - 1;
                    }
                }
            }

            return levels[levelKeys[levelIndex]];
        }
    }
}

