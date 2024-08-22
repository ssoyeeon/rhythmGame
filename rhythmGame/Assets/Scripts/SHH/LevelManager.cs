using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmGame
{
    [System.Serializable]
    public class LevelData
    {
        [SerializeField]
        private int highScore;                  // �ְ� ����
        [SerializeField]
        private int clearCount;                 // Ŭ���� Ƚ��
        [SerializeField]
        private int playCount;                  // �÷��� Ƚ��
        private LevelObject levelObject;        // �� ������ ������
        
        public LevelData(LevelObject _levelObject)
        {
            levelObject = _levelObject;
        }

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
        public LevelObject GetLevelObject()
        {
            return levelObject;
        }
    }
    public class LevelManager : MonoBehaviour
    {
        public List<LevelObject> levelObjects;              // ������ �������� ���� ����Ʈ
        private Dictionary<string, LevelData> levels = new Dictionary<string, LevelData>();      // ������
        private List<string> levelKeys;

        private int levelIndex;
        private int levelCount;

        void Awake()
        {
            if (levels.Count < 0 || levels == null)
            {
                AddLevelDataToDictionary();
            }
        }

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

        private void AddLevelDataToDictionary()
        {
            if (levelObjects.Count < 0)
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
                }
            }

            levelCount = levels.Count;
        }

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

